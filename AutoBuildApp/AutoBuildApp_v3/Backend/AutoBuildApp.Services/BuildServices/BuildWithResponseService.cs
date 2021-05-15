using System;
using System.Collections.Generic;
using System.Threading;
using AutoBuildApp.DataAccess;
using AutoBuildApp.DataAccess.Entities;
using AutoBuildApp.DomainModels;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Enumerations;

/**
* Build Management Service is a service that calls a 
* DAO and translates the returned data into a build type.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Services
{
    public class BuildWithResponseService
    {
        private BuildDAO _dao;
        private CommonResponse _response;

        public BuildWithResponseService(BuildDAO buildDAO)
        {
            _dao = buildDAO;
        }

        public CommonResponse AddBuild(Build build, string buildName, string user)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                response = _dao.InsertBuild(build, buildName, user);
            }
            catch (TimeoutException)
            {
                response.IsSuccessful = false;
                response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return response;
        }

        public CommonResponse CopyBuild()
        {
            _response = new CommonResponse();

            try
            {
                _response.IsSuccessful = true;
                _response.ResponseString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
            }
            catch (TimeoutException)
            {
                _response.IsSuccessful = false;
                _response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return _response;
        }

        public CommonResponse DeleteBuild(string buildName)
        {

            CommonResponse response = new CommonResponse();

            try
            {
                response = _dao.DeleteBuild(buildName, Thread.CurrentPrincipal.Identity.Name);
            }
            catch (TimeoutException)
            {
                response.IsSuccessful = false;
                response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return response;
        }


        public CommonResponse ModifyProductsInBuild(string buildName, string modleNumber, int quantity)
        {

            CommonResponse response = new CommonResponse();

            try
            {
                response = _dao.ModifyProductQuantityFromBuild(buildName, modleNumber, quantity, Thread.CurrentPrincipal.Identity.Name);
                return response;
            }
            catch (TimeoutException)
            {
                response.IsSuccessful = false;
                response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return response;
        }



        public CommonResponse AddRecomendedBuild
            (IList<string> modelNumbers, string buildName)
        {

            CommonResponse response = new CommonResponse();

            try
            {
                response = _dao.SaveBuildRecommended(modelNumbers, buildName, Thread.CurrentPrincipal.Identity.Name);
                return response;
            }
            catch (TimeoutException)
            {
                response.IsSuccessful = false;
                response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }
            catch( ArgumentNullException)
            {
                response.IsSuccessful = false;
                response.ResponseString = ResponseStringGlobals.INVALID_INPUT;

            }

            return response;
        }



        public Build GetBuild()
        {
            return null;
        }

        public List<Build> GetAllUserBuilds(string sortOrder)
        {

            try
            {
                var buildLists = _dao.GetListOfBuilds(Thread.CurrentPrincipal.Identity.Name);
                return buildLists;
            }
            catch (TimeoutException)
            {
                return null; 
            }
            catch (ArgumentNullException)
            {
                return null;
            }
            catch(NullReferenceException)
            {
                return null;
            }
        }

        public CommonResponse PublishBuild(BuildPost BuildPost)
        {

            BuildPostEntity BuildPostEntity = new BuildPostEntity()
            {
                Title = BuildPost.Title,
                Description = BuildPost.Description,
                BuildImagePath = BuildPost.BuildImagePath,
                DateTime = BuildPost.DateTime
            };
            CommonResponse response = new CommonResponse();
            try
            {
                var result = _dao.PublishBuild(BuildPostEntity, Thread.CurrentPrincipal.Identity.Name);
                
                if (result.Code == AutoBuildSystemCodes.Success)
                {
                    response.ResponseString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
                    response.IsSuccessful = true;
                }
                if (result.Code == AutoBuildSystemCodes.FailedParse)
                {
                    response.ResponseString = ResponseStringGlobals.FAILED_ADDITION;
                    response.IsSuccessful = false;
                }

                return response;
            }
            catch (TimeoutException)
            {
                response.IsSuccessful = false;
                response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }
            catch (ArgumentNullException)
            {
                response.IsSuccessful = false;
                response.ResponseString = ResponseStringGlobals.INVALID_INPUT;

            }
            return _response;
        }

        public CommonResponse ModifyBuild()
        {
            _response = new CommonResponse();

            return _response;
        }
    }
}
