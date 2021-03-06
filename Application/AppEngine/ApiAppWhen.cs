using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AppWebCustom;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.AppEngine
{
    public class ApiAppWhen
    {
         public static async Task<bool> Execute(AppAction appAction, DataContext _context, string CurrentUserId, UserManager<AppUser> _userManager)
         {            

             if( !string.IsNullOrEmpty(appAction.WhenXml))
             {
                #region Load XML

                XmlDocument whenNodeDoc = new XmlDocument();            
                try
                {
                    whenNodeDoc.LoadXml(appAction.WhenXml);
                }
                catch (Exception ex)
                {
                    throw new Exception( "invalid ActionXml" + ex.Message );
                }

                #endregion Load XML

                XmlNodeList whenNode = whenNodeDoc.GetElementsByTagName("When");    
                return await ExecuteWhen(whenNode.Item(0).FirstChild, _context, CurrentUserId, _userManager);
             }
             else{
                 return true;
             }             
         }
 
         public static async  Task<bool> ExecuteWhen(XmlNode whenNode, DataContext _context, string CurrentUserId, UserManager<AppUser> _userManager)
         {
            //bool result = false;           

            string itemName = whenNode.Name.ToLower();
            if (itemName == "and" || itemName == "or" )
            {
                bool res = false;                
                bool res1 = await ExecuteWhen(whenNode.ChildNodes[0], _context, CurrentUserId, _userManager);
                bool res2 = await ExecuteWhen(whenNode.ChildNodes[1], _context, CurrentUserId, _userManager);
                return res;
            }
            else{
               return await WhenActions.ExecuteWhenAction(whenNode, _context, CurrentUserId, _userManager);
            }                       
         }


    }
}