using System;
using System.Threading.Tasks;
using System.Xml;
using AppWebCustom.When;
using Domain;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace AppWebCustom
{
    public class WhenActions
    {
        public static async  Task<bool> ExecuteWhenAction(XmlNode ActionNode, DataContext _context, string CurrentUserId, UserManager<AppUser> _userManager)
        {  
            bool res = false;

            string itemName = ActionNode.Name.ToLower();

            try{
                switch(itemName){

                    case "userroles":
                        res = await userroles.Execute(ActionNode, _context, CurrentUserId, _userManager);
                        break;
                    case "datacomarison":
                        res = await datacomarison.Execute(ActionNode, _context, CurrentUserId);
                        break;                    
                    default:
                        return false;
                }
            }
            catch(Exception ex){
                 throw new Exception( $"Error while checking when -> {ex.Message }" );
            }

            return res;
        }
    }
}