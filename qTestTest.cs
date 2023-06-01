using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Constraints;
using PortalApp.Models;

namespace PortalApp
{
    public class qTestTest : TestFixture
    {

        public bool testPlan = false;
        public int testPlanId = 0;

        [TestCase(TestName = "Delete test case VSTS"), Order(6)]
        public void DeleteTestCaseVSTS(string vstsProjectId = "Mitel LiveChat")
        {

            qTest qtest = new qTest();
            //qtest.GetTestCases(qtestProjectId);

            for (int i = 19147; i <= 19216; i++)
            {

                //get all modules for proj
                qtest.DeleteTestCase(vstsProjectId, i.ToString());

                qtest.SetAuthorization("Basic",
                    "basicKey");
                var respo = qtest.Delete();
                //Assert.AreEqual(204, (int)respo.status, $"Status code is not 200");

                //document if not success
                if (!((int)respo.status).Equals(204))
                    TestContext.WriteLine(i + $" {(int)respo.status}");
            }
        }




        //[Ignore("Test for Q Test")]
        [TestCase(TestName = "Export test cases to VSTS from QTest with folder"), Order(2)]
        public void ExportTestCasesToVSTSWithFolder(string qtestProjectId = "171717",
            string vstsProjectId = "Proj Id")
        {

            qTest qtest = new qTest();
            //qtest.GetTestCases(qtestProjectId);

            //get all modules for proj
            qtest.GetModules(qtestProjectId);

            qtest.SetAuthorization(Constants.qtestAuthType, Constants.qtestKeyValue);
            //qtest.SetCustomHeader("Content-Type", "application/json");
            var respo = qtest.Get();
            Assert.AreEqual(200, (int) respo.status, $"Status code is not 200");

            var resp = JsonConvert.DeserializeObject<IList<AllModulesResp>>(respo.response);

            foreach (var module in resp)
                recursiveCreateTC(module, qtest, qtestProjectId, vstsProjectId, module.name);

        }

        public void recursiveCreateTC(AllModulesResp module, qTest qtest, string qtestProjectId, string vstsProjectId,
            string tags)
        {
            //create TCs for this module
            CreateTestCases(qtest, qtestProjectId, vstsProjectId, module.id.ToString(), tags);

            //create TCs for child module
            if (module.children != null)
                foreach (var subModule in module.children)
                {
                    //if (iIndex++ == 0)
                    //tags = tags + "\\" + subModule.name;

                    CreateTestCases(qtest, qtestProjectId, vstsProjectId, subModule.id.ToString(),
                        tags + "\\" + subModule.name);

                    if (subModule.children != null)
                        foreach (var mod in subModule.children)
                            recursiveCreateTC(mod, qtest, qtestProjectId, vstsProjectId,
                                tags + "\\" + subModule.name + "\\" + mod.name);
                }
        }

        public void CreateTestCases(qTest qtest, string qtestProjectId, string vstsProjectId, string moduleId,
            string tags)
        {

            //create main test plan
            if (!testPlan)
                createTestPlan(qtest, vstsProjectId);

            //create test suite(s)
            createQuerySuite(qtest, vstsProjectId, tags);

            //fetch test cases from qtest
            qtest.GetTestCases(qtestProjectId, moduleId);
            qtest.SetAuthorization(Constants.qtestAuthType, Constants.qtestKeyValue);
            var respo1 = qtest.Get();
            Assert.AreEqual(200, (int) respo1.status, $"Status code is not 200");

            var resp = JsonConvert.DeserializeObject<List<AllModulesResp>>(respo1.response);

            //migrate each testCase
            foreach (var testCase in resp)
                connTestCase(qtest, qtestProjectId, vstsProjectId, testCase.id, tags);
                //TestContext.WriteLine(testCase.name);

        }

        public void connTestCase(qTest qtest, string qtestProjectId, string vstsProjectId, int testCaseId, string tags)
        {

                TestContext.WriteLine(testCaseId);
                qtest.GetTestCaseDetails(qtestProjectId, testCaseId.ToString());
                qtest.SetAuthorization(Constants.qtestAuthType, Constants.qtestKeyValue);
                var respo = qtest.Get();

                Assert.AreEqual(200, (int) respo.status, $"Status code is not 200");

                var resp1 = JsonConvert.DeserializeObject<TestDetails>(respo.response);

                var iIndex = 1;
                var steps = "";
                string str = "";
                bool attchFlag = false;
                string title = "";

                List<FieldsAll> fields = new List<FieldsAll>();

                fields.Add(new Field() {op = "add", from = null, path = "/fields/System.Tags", value = tags});
                fields.Add(new Field() {op = "add", from = null, path = "/fields/System.AssignedTo", value = ""});

                fields.Add(new Field()
                    {op = "add", from = null, path = "/fields/Custom.AutomationRequired", value = "No"});

                //if (resp1.description.Trim().Length > 0)
                  //  str += formatString("Description", resp1.description);

                //loop on each property
                foreach (var property in resp1.properties)
                    str += formatString(property.field_name, property.field_value);

                //if (resp1.properties[6].field_name.Contains("Requirements"))
                //    str += formatString("Requirements", resp1.properties[6].field_value);

                //code for fetching main attachments
                //if (resp1.links.Count > 0)
                //  str = formatString("Description", resp1.description);

                fields.Add(new Field()
                {
                    op = "add", from = null, path = "/fields/System.Description",
                    value = resp1.description + " " + resp1.id + $" qTest link = https://company.qtestnet.com/p/{qtestProjectId}/portal/project#tab=testdesign&object=1&id={resp1.id}"
                });

                //attachment code

                //fields.Add(new Field() { op = "add", from = null, path = "/fields/System.Reason", value = resp1.id.ToString()});

                if (!resp1.test_steps.Count.Equals(0))
                {
                    if (resp1.precondition.Trim().Length > 0)
                        steps = steps +
                                $"<step id =\"{iIndex++}\" type=\"ValidateStep\"><parameterizedString isformatted =\"false\">Pre-condition: {sanitizeString(resp1.precondition)}</parameterizedString><parameterizedString/><description/></step>";

                    //create vsts test steps
                    foreach (var step in resp1.test_steps)
                    {

                        //strip html off
                        step.description = sanitizeString(step.description);
                        step.expected = sanitizeString(step.expected);

                        //add each step here
                        steps = steps +
                                $"<step id =\"{iIndex.ToString()}\" type=\"ValidateStep\"><parameterizedString isformatted =\"false\">{step.description}</parameterizedString>" +
                                $"<parameterizedString isformatted =\"false\">{step.expected}</parameterizedString><description/></step>";

                        //attachment
                        if (step.attachments.Count > 0)
                        {
                            str = connAttachments(qtest, step, fields, qtestProjectId, str, iIndex);
                            attchFlag = true;
                        }

                        ++iIndex;

                    }

                    //add prefix
                    steps = $"<steps id=\"0\" last=\"{iIndex.ToString()}\">" + steps + "</steps>";

                    fields.Add(new Field()
                        {op = "add", from = null, path = "/fields/Microsoft.VSTS.TCM.Steps", value = steps});
                }

                if (attchFlag)
                    //title = "[qTest] " + resp1.name + " [A]";
                    title = resp1.name + " [A]";
                else
                    //title = "[qTest] " + resp1.name;
                    title = resp1.name;

                fields.Add(new Field() {op = "add", from = null, path = "/fields/System.Title", value = title});

                //
                //if (attchFlag)
                // {
                //   fields.Add(new Field() { op = "add", from = null, path = "/relations/-", value = title });
                //}
                //str += $" qTest id = {resp1.id}";
                str += $"<br>qTest link = <a href=\"https://company.qtestnet.com/p/{qtestProjectId}/portal/project#tab=testdesign&object=1&id={resp1.id}\">{resp1.id}</a>";

                //add comments of str var
                fields.Add(new Field() {op = "add", from = null, path = "/fields/System.History", value = str});

                qtest.CreateTestCase(vstsProjectId, fields);
                qtest.SetAuthorization("Basic",
                    "basicKey");
                //qtest.SetAcceptHeader("application/json-patch+json");
                respo = qtest.Post();

                Assert.AreEqual(200, (int) respo.status, $"Status code is not 200");

                var resp2 = JsonConvert.DeserializeObject<CrtWrkItemResp>(respo.response);
                TestContext.WriteLine(resp2.id);

                System.Threading.Thread.Sleep(300);

            /*
                //add testcase to static test suite id
                qtest.AddTestsToSuite(vstsProjectId, testPlanId,te, resp2.id);
                qtest.SetAuthorization("Basic",
                    "cmJhbmdhbG86YjR1Z3NuYTM1cjNqNW82dzUzbmQ1cnhrc2pyaWV1aXY2ZnFtdGtsdXhhMnRpemZ0bGFkcQ==");
                //qtest.SetAcceptHeader("application/json-patch+json");
                respo = qtest.Post();

                Assert.AreEqual(200, (int) respo.status, $"Status code is not 200");
                */

        }

        public string connAttachments(qTest qtest, TestStep step, List<FieldsAll> fields, string qtestProjectId, string str, int iIndex)
        {
            foreach (var stepAttachment in step.attachments)
            {
                //stepAttachment.web_url
                //append step attchments URL to comments
                str += formatString($"Step {iIndex} attachment", stepAttachment.web_url);

                var file = getAttachment(qtest, qtestProjectId, step.id.ToString(), stepAttachment.id.ToString());
                //GenerateStreamFromString(file);

                UploadResp upldStr = uploadStream(file, qtest, stepAttachment.name);

                Value val = new Value();
                val.rel = "AttachedFile";
                val.url = upldStr.url;

                Attributes attr = new Attributes();
                attr.comment = $"[TestStep={iIndex}]:";

                val.attributes = attr;

                fields.Add(new FieldAttch() { op = "add", path = "/relations/-", value = val });
            }

            return str;
        }


        public void createTestPlan(qTest qtest, string vstsProjectId)
        {
            TestDetails resp1_ = null;

            //create TP only once
            {
                //create test plan
                TestPlan testPlan_ = new TestPlan();
                Area area = new Area();
                area.name = vstsProjectId;
                testPlan_.area = area;
                testPlan_.name = "qTest Test Plan";
                testPlan_.iteration = vstsProjectId;

                qtest.CreateTestPlan(vstsProjectId, testPlan_);
                qtest.SetAuthorization("Basic",
                    "someKey");
                //qtest.SetAcceptHeader("application/json-patch+json");
                var respo_1 = qtest.Post();

                Assert.AreEqual(200, (int) respo_1.status, $"Status code is not 200");
                resp1_ = JsonConvert.DeserializeObject<TestDetails>(respo_1.response);

                testPlan = true;
                testPlanId = resp1_.id;
            }
        }



        public void createQuerySuite(qTest qtest, string vstsProjectId, string tags)
        {
            //create query based test suite on tags
            TestSuite testSuite = new TestSuite();
            testSuite.name = tags;
            testSuite.suiteType = "DynamicTestSuite";
            testSuite.queryString =
                $"select [System.Id], [System.WorkItemType], [System.Description], [Microsoft.VSTS.Common.Priority], [System.AssignedTo], [System.AreaPath] from WorkItems where [System.TeamProject] = @project and [System.WorkItemType] in group 'Microsoft.TestCaseCategory' and [System.Description] contains words 'qTest' and [System.Tags] contains '{tags}'";
                //$"select [System.Id], [System.WorkItemType], [System.Title], [Microsoft.VSTS.Common.Priority], [System.AssignedTo], [System.AreaPath] from WorkItems where [System.TeamProject] = @project and [System.WorkItemType] in group 'Microsoft.TestCaseCategory' and [System.Title] contains words 'qTest' and [System.Tags] contains '{tags}'";

            qtest.CreateTestSuite(vstsProjectId, testSuite, testPlanId);
            qtest.SetAuthorization("Basic", "cmJhbmdhbG86YjR1Z3NuYTM1cjNqNW82dzUzbmQ1cnhrc2pyaWV1aXY2ZnFtdGtsdXhhMnRpemZ0bGFkcQ==");
            //qtest.SetAcceptHeader("application/json-patch+json");
            var respo_ = qtest.Post();

            Assert.AreEqual(200, (int)respo_.status, $"Status code is not 200");
            //resp1_ = JsonConvert.DeserializeObject<TestDetails>(respo_.response);

        }

        public void createStaticSuite(qTest qtest, string vstsProjectId, string tags)
        {
            //create query based test suite on tags
            TestSuite testSuite = new TestSuite();
            testSuite.name = tags;
            testSuite.suiteType = "DynamicTestSuite";
            testSuite.queryString =
                $"select [System.Id], [System.WorkItemType], [System.Title], [Microsoft.VSTS.Common.Priority], [System.AssignedTo], [System.AreaPath] from WorkItems where [System.TeamProject] = @project and [System.WorkItemType] in group 'Microsoft.TestCaseCategory' and [System.Title] contains words 'qTest' and [System.Tags] contains '{tags}'";

            qtest.CreateTestSuite(vstsProjectId, testSuite, testPlanId);
            qtest.SetAuthorization("Basic", "basicKey");
            //qtest.SetAcceptHeader("application/json-patch+json");
            var respo_ = qtest.Post();

            Assert.AreEqual(200, (int)respo_.status, $"Status code is not 200");
            //resp1_ = JsonConvert.DeserializeObject<TestDetails>(respo_.response);

        }



        public Stream getAttachment(qTest qtest, string qtestProjectId, string testStepId, string attchId)
        {

            qtest.GetAttachment(qtestProjectId, testStepId, attchId);
            //qtest.GetTestCaseDetails(qtestProjectId, testCase.id.ToString());
            qtest.SetAuthorization(Constants.qtestAuthType, Constants.qtestKeyValue);
            //qtest.SetCustomHeader("Cookie", "UserContextToken=OTNjYzNlMGItMWUyMS00YzQ3LTk1MjQtMTEzYzgyMzAzNTVl");
            var respo = qtest.GetStream();

            Assert.AreEqual(200, (int)respo.status, $"Status code is not 200");

            return respo.response;

            //var resp1 = JsonConvert.DeserializeObject<TestDetails>(respo.response);

        }

        public UploadResp uploadStream(Stream stream, qTest qtest, string fileName)
        {
            //qTest qtest = new qTest();
            qtest.UploadStream("Some text", stream, fileName);
            qtest.SetAuthorization("Basic", "basicKey");
            var respo = qtest.Post();

            Assert.AreEqual(201, (int)respo.status, $"Status code is not 200");

            var resp1 = JsonConvert.DeserializeObject<UploadResp>(respo.response);

            return resp1;
        }

        public static Stream GenerateStreamFromString(string str)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(str);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public string uploadAttachment(string areaPath, string fileName, qTest qtest)
        {
            //qTest qtest = new qTest();
            qtest.UploadAttachment("Some text", $@"C:\Users\user\Desktop\{fileName}");
            qtest.SetAuthorization("Basic", "basicKey");
            var respo = qtest.Post();

            Assert.AreEqual(201, (int)respo.status, $"Status code is not 200");

            var resp1 = JsonConvert.DeserializeObject<UploadResp>(respo.response);

            return resp1.id;
        }


        public string formatString(string title, string body)
        {
            string result = $"<br>{title}: <br>{body}<br>";
            return result;
        }

        public string sanitizeString(string str)
        {
            str = Regex.Replace(str, "<.*?>", String.Empty);
            str = str.Replace("&nbsp;", "");
            return str;
        }

        [TestCaseSource(typeof(Data), "ReleaseGates"), Order(2)]
        public void ReleaseGates(int status, string code, string batchId, string modelType, 
                string approvalStage, string cid)
        {
            code = "someKey";

            NextBus nextBus = new NextBus();
            //nextBus.SetCustomHeader(Constants.apiKeyParam, Constants.apiKeyValue);

            nextBus.ReleaseGates(code, batchId, modelType, approvalStage, cid);
            var respo = nextBus.Get();

            Assert.AreEqual(status, (int) respo.status, $"Status code is not {status}");

            //continue if fail
            if (!status.Equals(200))
                return;

            var resp = JsonConvert.DeserializeObject<ReleaseGate>(respo.response);
            //Assert.AreEqual(model.version, resp.version, $"Response model version {resp.version} does not match");
            Assert.AreEqual(cid, resp.cid, $"Cid - {resp.cid} do not match");

        }



    }
}
