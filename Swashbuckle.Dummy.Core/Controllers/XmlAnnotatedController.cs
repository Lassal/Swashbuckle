using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;

namespace Swashbuckle.Dummy.Controllers
{
    [RoutePrefix("xmlannotated")]
    public class XmlAnnotatedController : ApiController
    {
        /// <summary>
        /// Registers a new Account based on <paramref name="account"/>.
        /// </summary>
        /// <summary xml:lang="pt-BR">
        /// Registra uma nova conta baseada em <paramref name="account"/>.
        /// </summary>
        /// <remarks>Create an <see cref="Account"/> to access restricted resources</remarks>
        /// <param name="account">Details for the account to be created</param>
        /// <response code="201"><paramref name="account"/> created</response>
        /// <response code="400">Username already in use</response>
        public int Create(Account account)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates a SubAccount.
        /// </summary>
        /// <summary xml:lang="pt-BR">
        /// Atualiza uma subconta (SubAccount)
        /// </summary>
        [HttpPut]
        public int UpdateSubAccount(SubAccount account)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Search all registered accounts by keywords 
        /// </summary>
        /// <remarks>Restricted to admin users only</remarks>
        /// <param name="keywords">List of search keywords</param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Account> Search(IEnumerable<string> keywords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Filters account based on the given parameters.
        /// </summary>
        /// <param name="q">The search query on which to filter accounts</param>
        /// <param name="page">A complex type describing the paging to be used for the request</param>
        /// <returns></returns>
        [HttpGet]
        [Route("filter")]
        public IEnumerable<Account> Filter(string q, [FromUri]Page page)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Prevents the account from being used
        /// </summary>
        [HttpPut]
        [ActionName("put-on-hold")]
        public void PutOnHold(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a reward to an existing account
        /// </summary>
        /// <param name="reward"></param>
        [HttpPut]
        [Route("{id}/add-reward")]
        public void AddReward(int id, Reward<string> reward)
        {
            throw new NotImplementedException();
        }
    }

    public class Page
    {
        /// <summary>
        /// The maximum number of accounts to return
        /// </summary>
        /// <summary xml:lang="pt-BR">
        /// O número máximo de contas que podem ser retornadas
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Offset into the result
        /// </summary>
        /// <summary xml:lang="pt-BR">
        /// Deslocamento (offset) do resultado
        /// </summary>
        public int Offset { get; set; }
    }

    /// <summary>
    /// Account details
    /// </summary>
    /// <summary xml:lang="pt-BR">
    /// Detalhes da conta
    /// </summary>
    public class Account
    {
        /// <summary>
        /// The ID for Accounts is 5 digits long.
        /// </summary>
        /// <summary xml:lang="pt-BR">
        /// O ID para a conta é inteiro de 5 digitos
        /// </summary>
        public virtual int AccountID { get; set; }
        
        /// <summary>
        /// Uniquely identifies the account
        /// </summary>
        /// <summary xml:lang="pt-BR">
        /// Identifica unicamente a conta
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// For authentication
        /// </summary>
        /// <summary xml:lang="pt-BR">
        /// Senha para autenticação
        /// </summary>
        public string Password { get; set; }

        public AccountPreferences Preferences { get; set; }

        public class AccountPreferences
        {
            /// <summary>
            /// Provide a display name to use instead of Username when signed in
            /// </summary>
            public string DisplayName { get; set; }

            /// <summary>
            /// Flag to indicate if marketing emails may be sent
            /// </summary>
            [JsonProperty("allow-marketing-emails")]
            public string AllowMarketingEmails { get; set; }            
        }
    }

    /// <summary>
    /// A Sub-Type of Account
    /// </summary>
    public class SubAccount : Account
    {
        /// <summary>
        /// The Account ID for SubAccounts should be 7 digits.
        /// </summary>
        public override int AccountID { get; set; }
    }

    /// <summary>
    /// A redeemable reward
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Reward<T>
    {
        /// <summary>
        /// The monetary value of the reward 
        /// </summary>
        public decimal value;

        /// <summary>
        /// The reward type
        /// </summary>
        public T RewardType { get; set; }
    }
}