<<<<<<< HEAD
﻿using StagingWebApi.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
=======
﻿// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using System;
>>>>>>> 15898dffd7c655c67c3d2a9a02c8142b328fef7d
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace StagingWebApi.Controllers
{
    public class V3Controller : ApiController
    {
<<<<<<< HEAD
        [Route("stage/v3/{ownerName}/{stageName}/index.json")]
=======
        [Route("source/v3/{ownerName}/{stageName}/index.json")]
>>>>>>> 15898dffd7c655c67c3d2a9a02c8142b328fef7d
        [HttpGet]
        public async Task<HttpResponseMessage> GetStageIndex(string ownerName, string stageName)
        {
            try
            {
                StagePersistenceFactory factory = new StagePersistenceFactory();
                IStagePersistence persistence = factory.Create();
                if (!await persistence.ExistsStage(ownerName, stageName))
                {
                    return new HttpResponseMessage(HttpStatusCode.NotFound);
                }

                string authority = Request.RequestUri.GetLeftPart(UriPartial.Authority);

                Index index = new Index();

                string queryResource = string.Format("{0}/v3/query/{1}/{2}", authority, ownerName, stageName);
                index.Add(queryResource, "@type", "SearchQueryService/3.0.0-beta");
                index.Add(queryResource, "comment", "Query endpoint of NuGet Search service (primary)");

                string flatContainerResource = string.Format("{0}/v3/flat/{1}/{2}/", authority, ownerName, stageName);
                index.Add(flatContainerResource, "@type", "PackageBaseAddress/3.0.0");
                index.Add(flatContainerResource, "comment", "Base URL of Azure storage where NuGet package registration info for DNX is stored");

                string registrationsBaseUrlResource = string.Format("{0}/v3/registration/{1}/{2}/", authority, ownerName, stageName);
                index.Add(registrationsBaseUrlResource, "@type", "RegistrationsBaseUrl/3.0.0-beta");
                index.Add(registrationsBaseUrlResource, "comment", "Base URL of Azure storage where NuGet package registration info is stored used by Beta clients");

                string reportAbuseResource = "https://www.nuget.org/packages/{id}/{version}/ReportAbuse";
                index.Add(reportAbuseResource, "@type", "ReportAbuseUriTemplate/3.0.0-beta");
                index.Add(reportAbuseResource, "comment", "URI template used by NuGet Client to construct Report Abuse URL for packages");

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = Utils.CreateJsonContent(index.ToJson());
                return response;
            }
            catch (Exception e)
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(e.Message);
                return response;
            }
        }
    }
}
