
using System;
using System.Collections.Generic;
using System.IO;
using CommonLib.RestApiV1;
using Newtonsoft.Json;
using PortalApp.Models;

namespace PortalApp
{
    public class qTest : Rest
    {
        
        private string serverName = Constants.qTestServerName;
        private const string protocol = Constants.protocol;
        private const string version = Constants.qTestVersion;

        //GET calls
        public void GetTestCases(string projectId)
        {
            URLi = $"{protocol}://{serverName}/api/v{version}/projects/{projectId}/test-cases";
        }

        public void GetTestCaseDetails(string projectId, string testId)
        {
            URLi = $"{protocol}://{serverName}/api/v{version}/projects/{projectId}/test-cases/{testId}";
        }

        public void GetAttachment(string projectId, string testId)
        {
            URLi = $"{protocol}://{serverName}/api/v{version}/projects/{projectId}/test-cases/{testId}";
        }

        public void GetAttachment(string projectId, string testStepId, string attchId)
        {
            //URLi = url;
            URLi = $"{protocol}://{serverName}/api/v{version}/projects/{projectId}/test-steps/{testStepId}/attachments/{attchId}";
        }

        public void DeleteTestCase(string vstsprojectId, string workitemId)
        {
            URLi = $"{protocol}://dev.azure.com/translink/{vstsprojectId}/_apis/test/testcases/{workitemId}?api-version=5.0-preview.1";
        }

        public void GetModules(string projectId)
        {
            URLi = $"{protocol}://{serverName}/api/v{version}/projects/{projectId}/modules?expand=descendants";
        }

        public void GetTestCases(string projectId, string parentId = "", string page = "", string size = "500", string expandProps = "false", string expandSteps = "false")
        {
            int iIndex = 0;
            URLi = "";

            //"ss=abc&sk=xyz&fd=123&td=235&ui=jjj"

            if (parentId == "")
            {
                //URLi = $"{protocol}://{serverName}/v{version}/IncidentReport/getCount";
                URLi = $"{protocol}://{serverName}/api/v{version}/projects/{projectId}/test-cases?" + URLi;
                return;
            }

            if (parentId != "")
            {
                URLi = URLi + $"parentId={parentId}";
                iIndex++;
            }

            //page number
            if (page != "")
            {
                if (iIndex > 0)
                    URLi = URLi + "&";

                URLi = URLi + $"page={page}";
                iIndex++;
            }

            //page size
            if (size != "")
            {
                if (iIndex > 0)
                    URLi = URLi + "&";

                URLi = URLi + $"size={size}";
                iIndex++;
            }

            if (expandProps != "")
            {
                if (iIndex > 0)
                    URLi = URLi + "&";

                URLi = URLi + $"expandProps={expandProps}";
            }

            if (expandSteps != "")
            {
                if (iIndex > 0)
                    URLi = URLi + "&";

                URLi = URLi + $"expandSteps={expandSteps}";
            }

            //URLi = $"{protocol}://{serverName}/v{version}/IncidentReport/getCount?" + URLi;
            URLi = $"{protocol}://{serverName}/api/v{version}/projects/{projectId}/test-cases?" + URLi;

        }





        //POST call
        public void CreateTestCase(string vstsprojectId, List<FieldsAll> fields)
        {
            URLi = $"{protocol}://dev.azure.com/translink/{vstsprojectId}/_apis/wit/workitems/$test case?api-version=5.1";
            sMessage = JsonConvert.SerializeObject(fields);
            mediaType = "application/json-patch+json";
        }

        public void CreateTestPlan(string vstsprojectId, TestPlan testPlan)
        {
            URLi = $"{protocol}://dev.azure.com/translink/{vstsprojectId}/_apis/test/plans?api-version=5.0";
            sMessage = JsonConvert.SerializeObject(testPlan);
            mediaType = "application/json";
        }

        public void CreateTestSuite(string vstsprojectId, TestSuite testSuite, long testPlanId)
        {
            URLi = $"{protocol}://dev.azure.com/translink/{vstsprojectId}/_apis/test/Plans/{testPlanId}/suites/{++testPlanId}?api-version=5.0";
            sMessage = JsonConvert.SerializeObject(testSuite);
            mediaType = "application/json";
        }

        public void AddTestsToSuite(string vstsprojectId, long testPlanId, long suiteId, long testCaseIds)
        {
            URLi = $"{protocol}://dev.azure.com/translink/{vstsprojectId}/_apis/test/Plans/{testPlanId}/suites/{suiteId}testcases/{testCaseIds}?api-version=5.1";
            mediaType = "application/json";
        }

        public void UploadAttachment(string vstsprojectId, string fileName)
        {
            URLi = $"{protocol}://dev.azure.com/translink/_apis/wit/attachments?uploadType=Simple&areaPath={vstsprojectId}&fileName={fileName}&api-version=5.1";
            //sMessage = JsonConvert.SerializeObject(fields);
            //sFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            mediaType = "application/octet-stream";
        }

        public void UploadStream(string vstsprojectId, Stream stream, string filename = "Campaign.jpg")
        {
            URLi = $"{protocol}://dev.azure.com/translink/_apis/wit/attachments?uploadType=Simple&areaPath={vstsprojectId}&fileName={filename}&api-version=5.1";
            //sMessage = JsonConvert.SerializeObject(fields);
            sStream = stream;
            mediaType = "application/octet-stream";
        }

        public void AddAttachment(FieldAttch fieldAttch, Stream stream, string testId)
        {
            URLi = $"{protocol}://dev.azure.com/translink/_apis/wit/workitems/{testId}?api-version=5.1";
            sMessage = JsonConvert.SerializeObject(fieldAttch);
        }





        public void CampaignEmail(CampaignEmail email)
        {
            //URLi = $"{protocol}://{serverName}/api/CampaignEmailReception?code={emailKey}";
            sMessage = JsonConvert.SerializeObject(email);
        }



    }
}
