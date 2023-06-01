using System;
using System.Collections.Generic;

namespace PortalApp.Models
{
    public class Link
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class Property
    {
        public int field_id { get; set; }
        public string field_name { get; set; }
        public string field_value { get; set; }
        public string field_value_name { get; set; }
    }

    public class qTest_
    {
        public List<Link> links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public string pid { get; set; }
        public DateTime created_date { get; set; }
        public DateTime last_modified_date { get; set; }
        public List<Property> properties { get; set; }
        public string web_url { get; set; }
        public int parent_id { get; set; }
        public int test_case_version_id { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public string precondition { get; set; }
        public int creator_id { get; set; }
    }

    public class UploadResp
    {
        public string id { get; set; }
        public string url { get; set; }
    }

    public class TestDetails
    {
        public List<Link> links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public string pid { get; set; }
        public DateTime created_date { get; set; }
        public DateTime last_modified_date { get; set; }
        public List<Property> properties { get; set; }
        public string web_url { get; set; }
        public List<TestStep> test_steps { get; set; }
        public int parent_id { get; set; }
        public int test_case_version_id { get; set; }
        public string version { get; set; }
        public string description { get; set; }
        public string precondition { get; set; }
        public int creator_id { get; set; }
        public List<object> agent_ids { get; set; }
    }

    public class Link2
    {
        public string rel { get; set; }
        public string href { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class Attachment
    {
        public IList<Link> links { get; set; }
        public string name { get; set; }
        public string content_type { get; set; }
        public int id { get; set; }
        public string web_url { get; set; }
        public DateTime created_date { get; set; }
        public Author author { get; set; }
        public int artifact_id { get; set; }
    }

    public class TestStep
    {
        public List<Link2> links { get; set; }
        public int id { get; set; }
        public string description { get; set; }
        public string expected { get; set; }
        public int order { get; set; }
        public List<Attachment> attachments { get; set; }
        public string plain_value_text { get; set; }
    }

    public class AllModulesResp
    {
        public IList<Link> links { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int order { get; set; }
        public string pid { get; set; }
        public DateTime created_date { get; set; }
        public DateTime last_modified_date { get; set; }
        public int parent_id { get; set; }
        public string description { get; set; }
        public bool recursive { get; set; }
        public IList<AllModulesResp> children { get; set; }
    }


    //VSTS objs
    public class Field : FieldsAll
    {
        public string op { get; set; }
        public string path { get; set; }
        public string from { get; set; }
        public string value { get; set; }
    }

    public class Attributes
    {
        public string comment { get; set; }
    }

    public class Value
    {
        public string rel { get; set; }
        public string url { get; set; }
        public Attributes attributes { get; set; }
    }

    public class FieldAttch : FieldsAll
    {
        public string op { get; set; }
        public string path { get; set; }
        public Value value { get; set; }
    }

    public interface FieldsAll
    {
    }

    public class Avatar
    {
        public string href { get; set; }
    }

    public class Links
    {
        public Avatar avatar { get; set; }
    }

    public class SystemAssignedTo
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class SystemCreatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class SystemChangedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class MicrosoftVSTSCommonActivatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Area
    {
        public string name { get; set; }
    }

    public class TestPlan
    {
        public string name { get; set; }
        public Area area { get; set; }
        public string iteration { get; set; }
    }

    public class Project
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Area_
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class UpdatedBy
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public Links _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class Owner
    {
        public string displayName { get; set; }
        public string url { get; set; }
        public object _links { get; set; }
        public string id { get; set; }
        public string uniqueName { get; set; }
        public string imageUrl { get; set; }
        public string descriptor { get; set; }
    }

    public class RootSuite
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class TestOutcomeSettings
    {
        public bool syncOutcomeAcrossSuites { get; set; }
    }

    public class TestPlanResp
    {
        public int id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public Project project { get; set; }
        public Area_ area { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string iteration { get; set; }
        public DateTime updatedDate { get; set; }
        public UpdatedBy updatedBy { get; set; }
        public Owner owner { get; set; }
        public int revision { get; set; }
        public string state { get; set; }
        public RootSuite rootSuite { get; set; }
        public string clientUrl { get; set; }
        public TestOutcomeSettings testOutcomeSettings { get; set; }
    }

    public class TestSuite
    {
        public string suiteType { get; set; }
        public string name { get; set; }
        public string queryString { get; set; }
    }


    /*
    public class Fields
    {
    public string System.AreaPath { get; set; }
    public string System.TeamProject { get; set; }
    public string System.IterationPath { get; set; }
    public string System.WorkItemType { get; set; }
    public string System.State { get; set; }
    public string System.Reason { get; set; }
    public SystemAssignedTo System.AssignedTo { get; set; }
    public DateTime System.CreatedDate { get; set; }
    public SystemCreatedBy System.CreatedBy { get; set; }
    public DateTime System.ChangedDate { get; set; }
    public SystemChangedBy System.ChangedBy { get; set; }
    public int System.CommentCount { get; set; }
    public string System.Title { get; set; }
    public DateTime Microsoft.VSTS.Common.StateChangeDate { get; set; }
    public DateTime Microsoft.VSTS.Common.ActivatedDate { get; set; }
    public MicrosoftVSTSCommonActivatedBy Microsoft.VSTS.Common.ActivatedBy { get; set; }
    public int Microsoft.VSTS.Common.Priority { get; set; }
    public string Microsoft.VSTS.TCM.AutomationStatus { get; set; }
    public string Custom.AutomationRequired { get; set; }
    public string Custom.Automated { get; set; }
    public string Custom.NonAutoReason { get; set; }
    public string Microsoft.VSTS.TCM.Steps { get; set; }
    }
    */
    public class CrtWrkItemResp
    {
        public int id { get; set; }
        public int rev { get; set; }
        public object fields { get; set; }
        public Links _links { get; set; }
        public string url { get; set; }
    }


}