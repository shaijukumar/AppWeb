using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using Application._AppApi;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Persistence;
using AppWebCustom;

namespace Application.AppEngine
{
    
    public class AppApiDataAction
    {
        // public class AttachmentJson
        // {
        //     public string Action { get; set; }
        //     public int FileArrayId { get; set; }
        //     public int Id { get; set; }
        //     public string FileName { get; set; }
        //     public string Prop1 { get; set; }
        //     public string Prop2 { get; set; }
        //     public string Prop3 { get; set; }
        //     public string Prop4 { get; set; }
        //     public string Prop5 { get; set; }    
        //     public int AppDataColumn { get; set; }
            
            
        // }
 
        // public static async Task<Dictionary<string, List<object>>> Execute(string DataSource, AppAction appAction, AppData appData, XmlNode actionNode, DataContext _context, ActionCommand request, string currentUserId)
        // {
        //     #region init variables

        //     //string result = string.Empty;            
        //     List<object> actionResult = new List<object>(); 
        //     List<AttachmentJson> attachments = new List<AttachmentJson>();
            
        //     #endregion init variables


        //     if(DataSource == "InPutParm" ){
                
        //         #region Get InPutParm

        //         string InPutParm = AppParm.GetAttributeValue( actionNode, "InPutParm", true ) ;
        //         string paremData  = AppParm.GetRequestParmValue( request, InPutParm);

        //         if(string.IsNullOrEmpty(paremData))
        //         {
        //             throw new Exception("AppData parm is missing");
        //         }

        //         #endregion InPutParm

        //         #region get columns fron db

        //         var appColumns = await _context.AppColumnMasters
        //             .Where(x => x.TableID == appAction.TableId  ).ToListAsync();

        //         #endregion get columns fron db

        //         # region Required/Optional Columns from action

        //             Dictionary<string, int> reqCol = new Dictionary<string, int>();
        //             foreach (XmlNode columnNode in actionNode.ChildNodes)
        //             {
        //                 if (columnNode.Name.ToLower() == "col")
        //                 {                                
        //                     if(columnNode.Attributes["req"] != null && columnNode.Attributes["req"].Value == "true")
        //                     {
        //                         reqCol[columnNode.InnerText] = 1;
        //                     }
        //                     else
        //                     {
        //                         reqCol[columnNode.InnerText] = -1;
        //                     }
        //                 }                  
        //             }

        //         # endregion column from action


        //         #region parse data input                
                
        //         var appDataInPut = new AppData();

        //         Type appDataType = appData.GetType();
                 
        //         try
        //         {
        //             JObject jObject = JObject.Parse(paremData);
        //             foreach(object obj in jObject){
        //                 string key = ((System.Collections.Generic.KeyValuePair<string, Newtonsoft.Json.Linq.JToken>)obj).Key.ToString();
        //                 string value = ((System.Collections.Generic.KeyValuePair<string, Newtonsoft.Json.Linq.JToken>)obj).Value.ToString();

        //                  if( key == "Id" ){
        //                     PropertyInfo ap1 = appDataType.GetProperty(key);
        //                     ap1.SetValue (appData, Int32.Parse(value), null);                            
        //                 }

        //                 ///add only if rows are in AppData Required/Optional Columns action xml
        //                 if(reqCol.ContainsKey(key))
        //                 {
        //                     reqCol[key]++;
                                            
        //                     if( key == "Id" || key == "StatusId"){
        //                         PropertyInfo ap1 = appDataType.GetProperty(key);
        //                         ap1.SetValue (appData, Int32.Parse(value), null);                            
        //                     }
        //                     else
        //                     {                                
        //                         var col = appColumns.FindLast( x => x.Title == key );
        //                         if(col == null){
        //                             throw new Exception("invalid column " + key);
        //                         }
        //                         PropertyInfo ap1 = appDataType.GetProperty(col.AppDataFiled);
        //                         if(  col.Type == AppColumnType.Float ){
        //                             ap1.SetValue (appData,  float.Parse(value), null);
        //                         }
        //                         else if( col.Type == AppColumnType.LongNumber ){
        //                             ap1.SetValue (appData,  Int64.Parse(value), null);
        //                         }
        //                         else if( col.Type == AppColumnType.Number || col.Type == AppColumnType.Config ){
        //                             ap1.SetValue (appData,  Int32.Parse(value), null);
        //                         }
        //                         else if( col.Type == AppColumnType.Attachment ){
        //                             if(!string.IsNullOrEmpty(value)){
        //                                 List<AttachmentJson> att = JsonConvert.DeserializeObject<List<AttachmentJson>>(value);
        //                                 foreach( var a in att){
        //                                     a.AppDataColumn = col.Id;
        //                                 }
        //                                 attachments.AddRange(att);   
        //                             }                                                             
        //                         }
        //                         else{
        //                             ap1.SetValue (appData,  value, null);
        //                         }                            
        //                     }                                                                                                                       
        //                 }
        //             }
        //             appDataInPut = jObject.ToObject<AppData>();
        //         }
        //         catch(Exception ex){
        //             throw new Exception("invalid Parm" + ex.Message );
        //         }
                
        //         //Check all required columns added
        //         if ( reqCol.Any(v => v.Value == 1))
        //         {                           
        //              throw new Exception("Values for following fields are required : " 
        //                 + string.Join(", ", reqCol.Where(v => v.Value == 1).Select(x => x.Key).ToArray()) );                        
        //         }
               

        //         #endregion parse data input

                               

        //         #region set appData values for each columns
                
        //         // foreach(var col in appColumns){
        //         //     PropertyInfo ap1 = appDataType.GetProperty(col.AppDataFiled);
        //         //     ap1.SetValue (appData,  ap1.GetValue(appDataInPut), null); 
        //         // }

        //         appData.ModifiedBy = currentUserId;
        //         appData.ModifiedOn = DateTime.Now;

        //         #endregion set appData values for each columns

                


        //     }
        //     else if(DataSource == "StatusUpdate" ){

        //     }

        //     #region Create/Update   

        //     if( string.IsNullOrEmpty(appData.Id.ToString()) || appData.Id == 0 )
        //     {
        //         #region Create 

        //         appData.TableId = appAction.TableId;
        //         appData.StatusId = appAction.ToStatusId;
        //         appData.CreatedBy = currentUserId;                                               
        //         appData.CreatedOn = DateTime.Now;                                    

        //         _context.AppDatas.Add(appData);
        //         //var success = await _context.SaveChangesAsync() > 0;
        //         await _context.SaveChangesAsync();
                
        //         actionResult.Add(appData);

        //         //result = JsonConvert.SerializeObject(appData);  

        //         #endregion Create
        //     }
        //     else
        //     {       
        //         #region  Update

        //         appData.StatusId = appAction.ToStatusId;
        //         var success = await _context.SaveChangesAsync() > 0; 

        //         actionResult.Add(appData);
                    
        //         #endregion Update                                   
        //     }

        //     #endregion Create/Update

        //     # region Add/Delete Attachment
            
        //     foreach(var a in attachments){
                
        //         if(a.Action == "Create"){
        //             foreach(var file in request.FileList){
        //                 if(file.FileName == a.FileName){     

        //                     var rootPath = @"C:\Attachments";
        //                     var path = Path.Combine( rootPath, appData.TableId.ToString(), appData.Id.ToString(), a.AppDataColumn.ToString());
                                                        
        //                     if(!Directory.Exists(path)){
        //                         Directory.CreateDirectory(path);
        //                     }

        //                     path = Path.Combine(path, file.FileName);

        //                     using (var fileStream = new FileStream(path,FileMode.Create))
        //                     {
        //                         await file.CopyToAsync(fileStream);                     
        //                     }

        //                     var appAttachment = new AppAttachment
        //                     {
        //                         AppDataId  = appData.Id,
        //                         AppDataColumn = a.AppDataColumn,
        //                         FileName  = file.FileName,
        //                         Path  = Path.GetRelativePath(rootPath, path),
        //                         Prop1  = a.Prop1,
        //                         Prop2  = a.Prop2,
        //                         Prop3  = a.Prop3,
        //                         Prop4  = a.Prop4,
        //                         Prop5  = a.Prop5                  
        //                     };
        //                     _context.AppAttachments.Add(appAttachment);
        //                     var success = await _context.SaveChangesAsync() > 0;                            
        //                 }
        //             }
        //         }
        //     }

        //     # endregion Add/Delete Attachment
                       
        //     Dictionary<string, List<object>> result = new Dictionary<string, List<object> >();
        //     result.Add("Result"+ (result.Count+1).ToString(), actionResult );

        //     return result;

        // }
    }
}