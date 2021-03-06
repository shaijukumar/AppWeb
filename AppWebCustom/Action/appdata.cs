using System;
using System.Threading.Tasks;
using Domain;
using Persistence;
using AppWebCustom;
using System.Collections.Generic;
using AppWebCustom.Common;
using System.Xml;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.Net;

namespace AppWebCustom.Action
{
    public class appdata
    {
         public class AttachmentJson
        {
            public string Action { get; set; }
            public int FileArrayId { get; set; }
            public int Id { get; set; }
            public string FileName { get; set; }
            public string FilePath { get; set; }                        
            public string Prop1 { get; set; }
            public string Prop2 { get; set; }
            public string Prop3 { get; set; }
            public string Prop4 { get; set; }
            public string Prop5 { get; set; }    
            public int AppDataColumn { get; set; }            
            
        }

        public static async Task<bool>  Execute(ApiDetails ad, XmlNode actionNode, DataContext _context, ActionCommand request, string currentUserId)
        {
            
            #region init variables
            
                List<object> actionResult = new List<object>(); 
                List<AttachmentJson> attachments = new List<AttachmentJson>();
            
            #endregion init variables

            #region Get InPutParm

                string InPutParm = XMLParm.GetAttributeValue( actionNode, "InPutParm", true ) ;
                string paremData  = XMLParm.GetRequestParmValue( request, InPutParm);

            if(string.IsNullOrEmpty(paremData))
            {
                throw new Exception("AppData parm is missing");
            }

            #endregion InPutParm

            #region get columns fron db

                if(ad.appColumns == null){
                    ad.appColumns = await _context.AppColumnMasters
                        .Where(x => x.TableID == ad.appAction.TableId  ).ToListAsync();
                }
                 
            #endregion get columns fron db

            # region Required/Optional Columns from action

                Dictionary<string, int> reqCol = new Dictionary<string, int>();
                foreach (XmlNode columnNode in actionNode.ChildNodes)
                {
                    if (columnNode.Name.ToLower() == "col")
                    {                                
                        if(columnNode.Attributes["req"] != null && columnNode.Attributes["req"].Value == "true")
                        {
                            reqCol[columnNode.InnerText] = 1;
                        }
                        else
                        {
                            reqCol[columnNode.InnerText] = -1;
                        }
                    }                  
                }

            # endregion column from action
            
            #region set Modified info
                               
                ad.appData.ModifiedBy = currentUserId;
                ad.appData.ModifiedOn = DateTime.Now;
                ad.appData.StatusId = ad.appAction.ToStatusId;

            #endregion set Modified info

            #region parse data input                
                
                var appDataInPut = new AppData();

                Type appDataType = ad.appData.GetType();
                 
                try
                {
                    JObject jObject = JObject.Parse(paremData);
                    foreach(object obj in jObject){
                        string key = ((System.Collections.Generic.KeyValuePair<string, Newtonsoft.Json.Linq.JToken>)obj).Key.ToString();
                        string value = ((System.Collections.Generic.KeyValuePair<string, Newtonsoft.Json.Linq.JToken>)obj).Value.ToString();

                        if( key == "Id" ){
                            PropertyInfo ap1 = appDataType.GetProperty(key);
                            ap1.SetValue (ad.appData, Int32.Parse(value), null);                            
                        }
                        else   ///add only if rows are in AppData Required/Optional Columns action xml
                        {if(reqCol.ContainsKey(key))
                            reqCol[key]++;
                                            
                            if( key == "StatusId"){
                                // PropertyInfo ap1 = appDataType.GetProperty(key);
                                // ap1.SetValue (ad.appData, Int32.Parse(value), null);                            
                            }
                            else
                            {                                
                                var col = ad.appColumns.FindLast( x => x.Title == key );
                                if(col != null){
                                    //throw new Exception("invalid column " + key);
                                
                                    PropertyInfo ap1 = appDataType.GetProperty(col.AppDataFiled);
                                    if(  col.Type == AppColumnType.Float ){
                                        ap1.SetValue (ad.appData,  float.Parse(value), null);
                                    }
                                    else if( col.Type == AppColumnType.LongNumber ){
                                        ap1.SetValue (ad.appData,  Int64.Parse(value), null);
                                    }
                                    else if( col.Type == AppColumnType.Number  ){
                                        ap1.SetValue (ad.appData,  Int32.Parse(value), null);
                                    }
                                    else if( col.Type == AppColumnType.Bool ){
                                        ap1.SetValue (ad.appData,  bool.Parse(value), null);
                                    }  
                                    else if( col.Type == AppColumnType.DateTime ){
                                        try{
                                            ap1.SetValue (ad.appData, DateTime.Parse(value), null);
                                        }catch(Exception ex){
                                            throw new Exception( $"Invalid date {value} in {key}. Exception : {ex.Message}" );
                                        }                                        
                                    }     
                                    else if( col.Type == AppColumnType.Config ){
                                        string strArray = string.Empty;  
                                        if(!string.IsNullOrEmpty(value)){                                           
                                            List<object> lst = new List<object>();

                                             //IF SINGLE ITEM ADDD ARRAY
                                            if( !value.StartsWith('[')){
                                                value = $"[{value}]";
                                            }

                                            JArray array = JArray.Parse(value);
                                            foreach (JObject jObj in array.Children<JObject>())
                                            {
                                                string itmId = jObj["Id"].ToString();
                                                if( !string.IsNullOrEmpty(itmId) && itmId != "0"){  

                                                     var exits = await  _context.AppConfigs
                                                        .Where( x => x.Id == Int32.Parse(itmId) )
                                                        .AnyAsync(); 
                                                    if(exits){
                                                        lst.Add(itmId);  
                                                    }
                                                    else{
                                                        throw new Exception( $"Invalid config id {itmId} in {key}");
                                                    }
                                                }
                                               
                                            }
                                            strArray = string.Join( ",", lst.ToArray() ) ;                                            
                                        }
                                        ap1.SetValue (ad.appData,  strArray, null);                                        
                                    }  
                                    else if( col.Type == AppColumnType.User ){
                                        
                                        string strArray = string.Empty;  
                                        if(!string.IsNullOrEmpty(value)){                                           
                                            List<object> lst = new List<object>();

                                             //IF SINGLE ITEM ADDD ARRAY
                                            if( !value.StartsWith('[')){
                                                value = $"[{value}]";
                                            }

                                            JArray array = JArray.Parse(value);
                                            foreach (JObject jObj in array.Children<JObject>())
                                            {
                                                string itmId = jObj["Username"].ToString();
                                                if( !string.IsNullOrEmpty(itmId)){
                                                    var user = await _context.Users.FirstOrDefaultAsync( u => u.UserName == itmId  ); 
 
                                                    if(user != null){
                                                        lst.Add(itmId);  
                                                    }
                                                    else{
                                                        throw new Exception( $"Invalid user id {itmId} in {key}");
                                                    }
                                                }                                                
                                            }
                                            strArray = string.Join( ",", lst.ToArray() ) ;                                            
                                        }
                                        ap1.SetValue (ad.appData,  strArray, null); 

                                    }                                   
                                    else if( col.Type == AppColumnType.Role ){    
                                        string strGroups = string.Empty;    
                                        if(!string.IsNullOrEmpty(value)){    

                                            List<object> groups = new List<object>();

                                            //IF SINGLE ITEM ADDD ARRAY
                                            if( !value.StartsWith('[')){
                                                value = $"[{value}]";
                                            }
                                            
                                            JArray array = JArray.Parse(value);
                                            foreach (JObject grpObject in array.Children<JObject>())
                                            {
                                                string grpId = grpObject["Id"].ToString();

                                                 if( !string.IsNullOrEmpty(grpId)){
                                                    var roleExists = await  _context.AspNetRoles
                                                        .Where( x => x.Id == grpId )
                                                        .AnyAsync(); 
                                                    if(roleExists){
                                                        groups.Add(grpId);  
                                                    }
                                                    else{
                                                        throw new Exception( "Invalid user role ");
                                                    }
                                                }
                                                                                                                
                                            }        
                                            // string s = JsonConvert.SerializeObject(groups); 
                                            strGroups = string.Join( ",", groups.ToArray() ) ; 
                                        }
                                        ap1.SetValue (ad.appData,  strGroups, null);

                                    }

                                    else if( col.Type == AppColumnType.Attachment ){
                                        if(!string.IsNullOrEmpty(value)){
                                            List<AttachmentJson> att = JsonConvert.DeserializeObject<List<AttachmentJson>>(value);
                                            foreach( var a in att){                                                
                                                a.AppDataColumn = col.Id;
                                            }
                                            attachments.AddRange(att);   
                                        }                                                             
                                    }
                                    else{
                                        ap1.SetValue (ad.appData,  value, null);
                                    }
                                }                            
                            }                                                                                                                       
                        }
                    }
                    appDataInPut = jObject.ToObject<AppData>();
                }
                catch(Exception ex){
                    throw new Exception("invalid Parm" + ex.Message );
                }
                
                //Check all required columns added
                if ( reqCol.Any(v => v.Value == 1))
                {                           
                     throw new Exception("Values for following fields are required : " 
                        + string.Join(", ", reqCol.Where(v => v.Value == 1).Select(x => x.Key).ToArray()) );                        
                }
               

            #endregion parse data input
            
            #region Create/Update   

            if( string.IsNullOrEmpty(ad.appData.Id.ToString()) || ad.appData.Id == 0 )
            {
                #region Create 

                //ad.appData.TableItemId = await NextTableCouter(ad, _context); //Get next TableItemId
                ad.appData.TableId = ad.appAction.TableId;               
                ad.appData.CreatedBy = currentUserId;                                               
                ad.appData.CreatedOn = DateTime.Now;                                    

                _context.AppDatas.Add(ad.appData);
                //var success = await _context.SaveChangesAsync() > 0;

                try{
                    //await _context.SaveChangesAsync();
                    var success = await _context.SaveChangesAsync() > 0;
                    if(success){
                        ad.appData.TableItemId = await NextTableCouter(ad, _context);

                        try{
                            success = await _context.SaveChangesAsync() > 0;
                        }
                        catch(Exception ex){
                            var m = ex.Message;
                        }
                        
                    }
                }
                catch(Exception ex){
                   throw new Exception(ex.Message);
                }
               
                
                actionResult.Add(ad.appData);

                //result = JsonConvert.SerializeObject(appData);  

                #endregion Create
            }
            else
            {       
                #region  Update
                                
                try{
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex){
                   throw new Exception(ex.Message);
                }
                actionResult.Add(ad.appData);
                    
                #endregion Update                                   
            }

            #endregion Create/Update
            
             # region Add/Delete Attachment
            
            foreach(var a in attachments){
                
                if(a.Action == "Create"){
                    foreach(var file in request.FileList){
                        if(file.FileName == a.FilePath){     

                            string rootPath = await Config.GetCongfigAsync("ApplicationConfig", "AttachmentPath", _context, true ); 

                            var path = Path.Combine( rootPath, ad.appData.TableId.ToString(), ad.appData.Id.ToString(), a.AppDataColumn.ToString());
                                                        
                            if(!Directory.Exists(path)){
                                Directory.CreateDirectory(path);
                            }

                            path = Path.Combine(path, file.FileName);

                            using (var fileStream = new FileStream(path,FileMode.Create))
                            {                                
                                await file.CopyToAsync(fileStream);                     
                            }

                            var appAttachment = new AppAttachment
                            {
                                AppDataId  = ad.appData.Id,
                                AppDataColumn = a.AppDataColumn,
                                FileName  = a.FileName,
                                Path  = Path.GetRelativePath(rootPath, path),
                                Prop1  = a.Prop1,
                                Prop2  = a.Prop2,
                                Prop3  = a.Prop3,
                                Prop4  = a.Prop4,
                                Prop5  = a.Prop5                  
                            };
                            _context.AppAttachments.Add(appAttachment);
                            try{
                                await _context.SaveChangesAsync();
                            }
                            catch(Exception ex){
                                throw new Exception(ex.Message);
                            }                            
                        }
                    }
                }
                else if(a.Action == "Delete"){
                                             
                     var appAttach = await _context.AppAttachments
                        .FindAsync(a.Id);

                    if (appAttach != null){

                        string rootPath = await Config.GetCongfigAsync("ApplicationConfig", "AttachmentPath", _context, true ); 
                        var path = Path.Combine(rootPath, appAttach.Path);

                        if (File.Exists(Path.Combine(path)))    
                        {                                
                            File.Delete(Path.Combine(path));                               
                        }    


                         _context.Remove(appAttach);
                         await _context.SaveChangesAsync();
                    }                                  
                }
                 else if(a.Action == "Update"){

                     var appAttach = await _context.AppAttachments
                        .FindAsync(a.Id);

                    if (appAttach != null){
                        
                        appAttach.Prop1  = a.Prop1 ?? appAttach.Prop1;
                        appAttach.Prop2  = a.Prop2 ?? appAttach.Prop2;
                        appAttach.Prop3  = a.Prop3 ?? appAttach.Prop3;
                        appAttach.Prop4  = a.Prop4 ?? appAttach.Prop4;
                        appAttach.Prop5  = a.Prop5 ?? appAttach.Prop5;
                        await _context.SaveChangesAsync();
                    }                                  
                }
            }

            # endregion Add/Delete Attachment
           
            return true;
        }

        public static async Task<int>  NextTableCouter(ApiDetails ad, DataContext _context)
        {                        
            var tableCouter = await _context.AppTableCouters.Where(c => c.TableId == ad.appAction.TableId).FirstOrDefaultAsync();
            tableCouter.counter += 1;
            //await _context.SaveChangesAsync();

            return Convert.ToInt32(tableCouter.counter);
        }
    }
}