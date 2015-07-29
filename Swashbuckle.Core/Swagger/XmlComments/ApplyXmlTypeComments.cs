using System;
using System.Xml.XPath;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Swagger;
using System.Globalization;
using System.Xml;

namespace Swashbuckle.Swagger.XmlComments
{
    public class ApplyXmlTypeComments : IModelFilter
    {
        private const string TypeExpression = "/doc/members/member[@name='T:{0}']";
        private const string SummaryExpression = "summary";
        private const string PropertyExpression = "/doc/members/member[@name='P:{0}.{1}']";
        private bool I18nEnabled = false;
        private string LocalizedSummaryExpression = null;
        private XmlNamespaceManager _namespaceManager;

        private readonly XPathNavigator _navigator;

        public ApplyXmlTypeComments(string xmlCommentsPath, CultureInfo culture)
        {
            if (culture != null)
            {
                this.I18nEnabled = true;
                this.LocalizedSummaryExpression = String.Format("summary[@xml:lang='{0}']", culture.Name);
            }
            _navigator = new XPathDocument(xmlCommentsPath).CreateNavigator();
            this._namespaceManager = new XmlNamespaceManager(this._navigator.NameTable);
            this._namespaceManager.AddNamespace("xml", "http://www.w3.org/XML/1998/namespace");
        }

        public ApplyXmlTypeComments(string xmlCommentsPath)
        {
            _navigator = new XPathDocument(xmlCommentsPath).CreateNavigator();
        }

        public void Apply(Schema model, ModelFilterContext context)
        {
            var typeNode = _navigator.SelectSingleNode(
                String.Format(TypeExpression, context.SystemType.XmlLookupName()));

            if (typeNode != null)
            {
                var summaryNode = this.GetSummaryNode(typeNode);
                if (summaryNode != null)
                    model.description = summaryNode.ExtractContent();
            }

            foreach (var entry in model.properties)
            {
                var jsonProperty = context.JsonObjectContract.Properties[entry.Key];
                if (jsonProperty == null) continue;

                ApplyPropertyComments(entry.Value, jsonProperty.PropertyInfo());
            }
        }

        private void ApplyPropertyComments(Schema propertySchema, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) return;

            var propertyNode = _navigator.SelectSingleNode(
                String.Format(PropertyExpression, propertyInfo.DeclaringType.XmlLookupName(), propertyInfo.Name));
            if (propertyNode == null) return;

            var propSummaryNode = this.GetSummaryNode(propertyNode);
            if (propSummaryNode != null)
            {
                propertySchema.description = propSummaryNode.ExtractContent();
            }
        }

        private XPathNavigator GetSummaryNode(XPathNavigator methodNode)
        {
            XPathNavigator summaryNode = null;

            if (this.I18nEnabled)
            {
                summaryNode = methodNode.SelectSingleNode(this.LocalizedSummaryExpression,this._namespaceManager);
                if (summaryNode != null)
                    return summaryNode;
            }

            return methodNode.SelectSingleNode(SummaryExpression);
        }

    }
}