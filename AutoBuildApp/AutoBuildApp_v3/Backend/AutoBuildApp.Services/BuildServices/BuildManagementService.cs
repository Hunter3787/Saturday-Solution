using System;
using System.Collections.Generic;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models;
using AutoBuildApp.Models.Interfaces;
/**
* Build Management Service is a service that calls a 
* DAO and translates the returned data into a build type.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Services
{
    public class BuildManagementService
    {
        private BuildDAO _dao;
        private IMessageResponse _response;

        public BuildManagementService(BuildDAO buildDAO)
        {
            _dao = buildDAO;
        }

        public IMessageResponse AddBuild(IBuild build, string buildName, string user)
        {
            _response = new StringBoolResponse();

            try
            {
                _dao.InsertBuild(build, buildName, user);


                _response.SuccessBool = true;
                _response.MessageString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
            }
            catch (TimeoutException)
            {
                _response.SuccessBool = false;
                _response.MessageString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return _response;
        }

        public IMessageResponse CopyBuild()
        {
            _response = new StringBoolResponse();

            try
            {
                _response.SuccessBool = true;
                _response.MessageString = ResponseStringGlobals.SUCCESSFUL_ADDITION;
            }
            catch (TimeoutException)
            {
                _response.SuccessBool = false;
                _response.MessageString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return _response;
        }

        public IMessageResponse DeleteBuild()
        {
            _response = new StringBoolResponse();

            try
            {
                _response.SuccessBool = true;
                _response.MessageString = ResponseStringGlobals.SUCCESSFUL_DELETION;
            }
            catch (TimeoutException)
            {
                _response.SuccessBool = false;
                _response.MessageString = ResponseStringGlobals.DATABASE_TIMEOUT;
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

        public IMessageResponse PublishBuild()
        {
            _response = new StringBoolResponse();

            return _response;
        }

        public IMessageResponse ModifyBuild()
        {
            _response = new StringBoolResponse();

            return _response;
        }
    }
}
