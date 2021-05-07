using System;
using System.Collections.Generic;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.Models.Interfaces;
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

        public CommonResponse AddBuild(IBuild build, string buildName, string user)
        {
            _response = new CommonResponse();

            try
            {
                _dao.InsertBuild(build, buildName, user);


                _response.ResponseBool = true;
                _response.ResponseString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
            }
            catch (TimeoutException)
            {
                _response.ResponseBool = false;
                _response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return _response;
        }

        public CommonResponse CopyBuild()
        {
            _response = new CommonResponse();

            try
            {
                _response.ResponseBool = true;
                _response.ResponseString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
            }
            catch (TimeoutException)
            {
                _response.ResponseBool = false;
                _response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return _response;
        }

        public CommonResponse DeleteBuild()
        {
            _response = new CommonResponse();

            try
            {
                _response.ResponseBool = true;
                _response.ResponseString = ResponseStringGlobals.SUCCESSFUL_DELETION;
            }
            catch (TimeoutException)
            {
                _response.ResponseBool = false;
                _response.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return _response;
        }

        public IBuild GetBuild()
        {
            return null;
        }

        public List<IBuild> GetAllUserBuilds(string user, string sortOrder)
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
