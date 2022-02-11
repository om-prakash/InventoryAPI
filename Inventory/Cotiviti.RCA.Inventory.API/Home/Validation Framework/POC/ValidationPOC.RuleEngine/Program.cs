using NRules;
using NRules.Fluent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ValidationPOC.Domains;

namespace ValidationPOC.RuleEngine
{
    class Program
    {
        static void Main(string[] args)
        {
            // Init rule engine sesssion dictionary
            Dictionary<int, ISession> sessions = InitSessionDictionary();

            Claim claim;
            IEnumerable<Claim> claims;

            #region Single Fact Validation for WellPoint
            // Generate an instance of Claim. This object works as a fact
            // which passed into NRules engine
            claim = FactGenerator.GenerateClaimFact(1847);

            // Validate the fact object via NRules engine
            Validate(sessions, claim);
                      
            // Check result
            if (claim.GetError().Length > 0)
            {
                Console.WriteLine($"Validation Failed. The validation error is: {Environment.NewLine}{claim.GetError()}");
            } else
            {
                Console.WriteLine("Validation Succeed.");
            }
            #endregion

            #region Multiple Facts Validation for WellPoint
            // Generate a list of Claim instance. This list works as a list of fact
            // which passed into NRules engine
            claims = FactGenerator.GenerateClaimListFact(10, 1847);

            // Validate the fact object via NRules engine
            Validate(sessions, claims);

            // Check result
            foreach(Claim item in claims)
            {
                if (item.GetError().Length > 0)
                {
                    Console.WriteLine($"Validation Failed. The validation error is: {Environment.NewLine}{item.GetError()}");
                }
                else
                {
                    Console.WriteLine("Validation Succeed.");
                }
            }
            #endregion

            #region Single Fact Validation for Gateway
            // Generate an instance of Claim. This object works as a fact
            // which passed into NRules engine
            claim = FactGenerator.GenerateClaimFact(693);

            // Validate the fact object via NRules engine
            Validate(sessions, claim);

            // Check result
            if (claim.GetError().Length > 0)
            {
                Console.WriteLine($"Validation Failed. The validation error is: {Environment.NewLine}{claim.GetError()}");
            }
            else
            {
                Console.WriteLine("Validation Succeed.");
            }
            #endregion

            #region Multiple Facts Validation for Gateway
            // Generate a list of Claim instance. This list works as a list of fact
            // which passed into NRules engine
            claims = FactGenerator.GenerateClaimListFact(10, 693);

            // Validate the fact object via NRules engine
            Validate(sessions, claims);

            // Check result
            foreach (Claim item in claims)
            {
                if (item.GetError().Length > 0)
                {
                    Console.WriteLine($"Validation Failed. The validation error is: {Environment.NewLine}{item.GetError()}");
                }
                else
                {
                    Console.WriteLine("Validation Succeed.");
                }
            }
            #endregion

            Console.ReadLine();
        }

        /// <summary>
        /// Build an instance of <seealso cref="ISession"/> from a rule assembly
        /// <para>
        /// Usually we create a dedicated assembly for a client, for instance: WellPoint and Wellcare
        /// should have different rule assembly
        /// </para>
        /// </summary>
        /// <param name="assembly">An instance of <seealso cref="Assembly"/> which contains rule definitions</param>
        /// <returns>An instance of <seealso cref="ISession"/></returns>
        private static ISession BuildSession(Assembly assembly)
        {
            var repository = new RuleRepository();
            // load rule assembly into rule repository
            repository.Load(x => x.From(assembly));
            // compile rules and create a session to be used
            return repository.Compile().CreateSession();
        }

        private static Dictionary<int, ISession> InitSessionDictionary()
        {
            Dictionary<int, ISession> sessions = new Dictionary<int, ISession>();
            // scan the folder of rule assembly and create an instance of ISession
            // based on each assemby that found and load the created ISession instance into
            // dictionary. By convention, the name of assembly is the id of company (client)
            string ruleAssemblyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Rules");
            string[] assemblies = Directory.GetFiles(ruleAssemblyPath, "*.dll");
            int companyId;
            FileInfo fi;
            foreach(string assembly in assemblies)
            {
                fi = new FileInfo(assembly);
                // get the company id from the file name of assembly
                // we assume the assembly follows our naming convention <companyid>.dll
                companyId = Convert.ToInt32(fi.Name.Replace(fi.Extension, string.Empty));
                sessions.Add(companyId, BuildSession(Assembly.LoadFrom(assembly)));
            }

            if (sessions.Count == 0)
            {
                throw new Exception("No rule assemblies found.");
            }

            return sessions;
        }

        /// <summary>
        /// Validate a single claim via NRules engine
        /// <para>
        /// A standard validation process is below:
        /// 1. insert the fact (the domain model object) into NRules session
        /// 2. call the Fire method to trigger the validation
        /// 3. retract the fact from the working memory of session to save used memory
        /// </para>
        /// </summary>
        /// <param name="sessions">Rule engine session dictionary</param>
        /// <param name="claim">An instance of <seealso cref="Claim"/></param>
        private static void Validate(Dictionary<int, ISession>  sessions, Claim claim)
        {
            ISession session = sessions[claim.CompanyID];
            if (session == null)
            {
                throw new Exception($"No rule engine session found for the specified company id {claim.CompanyID}");
            }
            session.Insert(claim);
            session.Fire();
            session.Retract(claim);
        }

        /// <summary>
        /// Validate a list of claim via NRules engine
        /// <para>
        /// A standard validation process is below:
        /// 1. insert the fact (the domain model object) into NRules session
        /// 2. call the Fire method to trigger the validation
        /// 3. retract the fact from the working memory of session to save used memory
        /// </para>
        /// </summary>
        /// <param name="sessions">Rule engine session dictionary</param>
        /// <param name="claims">A list of instance of <seealso cref="Claim"/></param>
        private static void Validate(Dictionary<int, ISession> sessions, IEnumerable<Claim> claims)
        {
            // group by company id
            var query = from claim in claims
                        group claim by claim.CompanyID into newGroup
                        orderby newGroup.Key
                        select newGroup;
            ISession session;
            foreach(var group in query)
            {
                session = sessions[group.Key];
                if (session == null)
                {
                    throw new Exception($"No rule engine session found for the specified company id {group.Key}");
                }
                session.InsertAll(group);
                session.Fire();
                session.RetractAll(group);
            }
        }
    }
}
