using System;
using System.Collections.Generic;
using System.Threading;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Builds;
using AutoBuildApp.Models.DataTransferObjects;

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


                //response.IsSuccessful = true;
                //response.ResponseString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
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



        public Build GetBuild()
        {
            return null;
        }

        public List<Build> GetAllUserBuilds(string user, string sortOrder)
        {

            return null;
        }

        public CommonResponse PublishBuild()
        {
            _response = new CommonResponse();

            return _response;
        }

        public CommonResponse ModifyBuild()
        {
            _response = new CommonResponse();

            return _response;
        }
    }
}
