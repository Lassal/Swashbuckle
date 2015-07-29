using System;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;
using Swashbuckle.Application;
using Swashbuckle.Dummy.Controllers;
using Swashbuckle.Tests.Swagger;
using System.Globalization;

namespace Swashbuckle.Tests.Swagger
{
    [TestFixture]
    public class XmlComments_i18nTests : SwaggerTestBase
    {

        public XmlComments_i18nTests(): base("swagger/docs/{apiVersion}")
        {
        }

        [SetUp]
        public void SetUp()
        {
            CultureInfo commentsCulture = CultureInfo.GetCultureInfo("pt-BR");

            SetUpAttributeRoutesFrom(typeof(XmlAnnotatedController).Assembly);
            SetUpDefaultRouteFor<XmlAnnotatedController>();
            SetUpHandler(c => c.IncludeXmlComments(String.Format(@"{0}\XmlComments.xml", AppDomain.CurrentDomain.BaseDirectory),commentsCulture));
        }

        [Test]
        public void It_documents_operations_from_action_summary_and_remarks_tags_including_paramrefs()
        {
            var swagger = GetContent<JObject>("http://tempuri.org/swagger/docs/v1");

            var postOp = swagger["paths"]["/xmlannotated"]["post"];

            Assert.IsNotNull(postOp["summary"]);
            Assert.AreEqual("Registra uma nova conta baseada em {account}.", postOp["summary"].ToString());

            Assert.IsNotNull(postOp["description"]);
            Assert.AreEqual("Create an {Swashbuckle.Dummy.Controllers.Account} to access restricted resources", postOp["description"].ToString());
        }


        [Test]
        public void It_documents_schemas_from_type_summary_tags()
        {
            var swagger = GetContent<JObject>("http://tempuri.org/swagger/docs/v1");

            var accountSchema = swagger["definitions"]["Account"];

            Assert.IsNotNull(accountSchema["description"]);
            Assert.AreEqual("Detalhes da conta", accountSchema["description"].ToString());
        }

        [Test]
        public void It_documents_schema_properties_from_property_summary_tags()
        {
            var swagger = GetContent<JObject>("http://tempuri.org/swagger/docs/v1");

            var usernameProperty = swagger["definitions"]["Account"]["properties"]["Username"];
            Assert.IsNotNull(usernameProperty["description"]);
            Assert.AreEqual("Identifica unicamente a conta", usernameProperty["description"].ToString());

            var passwordProperty = swagger["definitions"]["Account"]["properties"]["Password"];
            Assert.IsNotNull(passwordProperty["description"]);
            Assert.AreEqual("Senha para autenticação", passwordProperty["description"].ToString());
        }

    }
}
