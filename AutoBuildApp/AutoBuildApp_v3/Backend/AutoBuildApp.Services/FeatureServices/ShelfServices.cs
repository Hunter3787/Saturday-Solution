using System;
using System.Collections.Generic;
using AutoBuildApp.Models;
using AutoBuildApp.Models.DataTransferObjects;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Models.Interfaces;

/**
* The ShelfServices will provide the shelf specific operatons.
* Catches errors, prepares commmon response object.
* @Author Nick Marshall-Eminger
*/
namespace AutoBuildApp.Services
{
    public class ShelfService
    {
        private ShelfDAO _dao;

        public ShelfService(ShelfDAO shelfDAO)
        {
            _dao = shelfDAO;
        }

        public CommonResponse CreateShelf(string shelfName, string user)
        {
            CommonResponse output = new CommonResponse();

            try
            {
                output.ResponseBool = _dao.InsertShelf(shelfName, user);
                output.ResponseString = ResponseStringGlobals.SUCCESSFUL_CREATION;
            }
            catch (TimeoutException)
            {
                output.ResponseBool = false;
                output.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return output;
        }

        public CommonResponse DeleteShelf(string shelfID)
        {
            CommonResponse output = new CommonResponse();
            try
            {
                //output.ResponseBool = _dao.DeleteShelf(shelfID);
                output.ResponseString = ResponseStringGlobals.SUCCESSFUL_DELETION;
            }
            catch (TimeoutException)
            {
                output.ResponseBool = false;
                output.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return output;
        }

        public CommonResponse ChangeShelfName(string oldName, string newName, string user)
        {
            CommonResponse output = new CommonResponse();
            try
            {
                //output.ResponseBool = _dao.UpdateShelf(oldName, newName, user);

                if(output.ResponseBool == false)
                {
                    output.ResponseString = ResponseStringGlobals.FAILED_MODIFICATION;
                }
                else
                { 
                    output.ResponseString = ResponseStringGlobals.SUCCESSFUL_MODIFICATION;
                }
            }
            catch (TimeoutException)
            {
                output.ResponseBool = false;
                output.ResponseString = ResponseStringGlobals.DATABASE_TIMEOUT;
            }

            return output;
        }

        public CommonResponse AddToShelf(IComponent item, string shelfName, string user)
        {
            CommonResponse output = new CommonResponse();
            return output;
        }

        public CommonResponse RemoveFromShelf(int index, string shelfName)
        {
            CommonResponse output = new CommonResponse();
            return output;
        }

        public CommonResponse ModifyShelf(int indexStart, int indexEnd, string user)
        {
            CommonResponse output = new CommonResponse();
            return output;
        }

        public CommonResponse ChangeQuantity(int count, string itemID, string shelfName)
        {
            CommonResponse output = new CommonResponse();
            return output;
        }

        public CommonResponseWithObject<Shelf> GetShelfByName(string shelfName, string user)
        {
            CommonResponseWithObject<Shelf> outputList = new CommonResponseWithObject<Shelf>();

            return outputList;
        }

        public List<Shelf> GetAllUserShelves(string userName)
        {
            List<Shelf> outputList = new List<Shelf>();

            return outputList;
        }

        public IComponent GetComponent(int index, string shelfName)
        {
            IComponent output = null;

            return output;
        }
    }
}