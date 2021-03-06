import { makeObservable, observable } from "mobx";
import { createContext } from "react";
import agent from "../../app/api/agent";
import { Attachment } from "../../app/common/form/MyAttachment";


//==============================================


export const ActionConfig : {[key: string]: number} = 
{
	ConfigCountries : 3,

	NavigationFlowId : 1,
	NavigationList : 1,
	NavigationById : 7,    
	NavigationTableID: 1,

	EmployeeFlowId : 5,
	EmployeeList : 19,
	EmployeeById : 21,    
	EmployeeTableID: 7,
	EmployeePassportDownload: 38,

}

export interface IAppApi {
	Id: number
	CustomerName: string
	CIF: number
}

export class AppApi implements IAppApi {
	Id: number = 0;
	CustomerName: string = '';
	CIF: number = 0;
  
  constructor(init?: IAppApi) {
    Object.assign(this, init);
  }
}

export interface IApiAction {	
	ActionId : number 
	ItemId : number
	ReturnFlow : string 
	Parm1 : string 
	Parm2 : string 
	Parm3 : string 
	Parm4 : string 
	Parm5 : string 
	Parm6 : string 
	Parm7 : string 
	Parm8 : string 
	Parm9 : string 
	Parm10 : string 	
	
}
 
export class AppApiAction implements IApiAction {
	ActionUniqName: string = '';
	ActionId : number = 0;
	ItemId : number = 0;
	ReturnActions : boolean = false;
	ReturnFlow : string = '';
	Parm1 : string = '';
	Parm2 : string = '';
	Parm3 : string = '';
	Parm4 : string = '';
	Parm5 : string = '';
	Parm6 : string = '';
	Parm7 : string = '';
	Parm8 : string = '';
	Parm9 : string = '';
	Parm10  : string = '';
	

  
  constructor(init?: IApiAction) {
    Object.assign(this, init);
  }
}

export interface IApiResult  {
	Id: number
	Result1: any[]
}

export class DataResult implements IApiResult {
	Id: number = 0;
	Result1: any[] = [];
  
  constructor(init?: IApiResult ) {
    Object.assign(this, init);
  }
}

export interface IApiResult  {
	Id: number
	Result1: any[]
}

export class ApiResult implements IApiResult {
	Id: number = 0;
	Result1: any[] = [];
  
  constructor(init?: IApiResult ) {
    Object.assign(this, init);
  }
}


export interface IAppStatusList {
	Id: number
	Title: string
	Order: number
	TableId: number
}

export interface IAppStatusList {
	Id: number
	Title: string
	Order: number
	TableId: number
}

export class AppStatusList implements IAppStatusList {
	Id: number = 0;
	Title: string = '';
	Order: number = 0;
	TableId: number = 0;
  
  constructor(init?: IAppStatusList) {
    Object.assign(this, init);
  }
}

export interface IAppUserRoleMaster {
	Id: string
	Name: string
}

export class AppUserRoleMaster implements IAppUserRoleMaster {
	Id: string = '';
	Name: string = '';
  
  constructor(init?: IAppUserRoleMaster) {
    Object.assign(this, init);
  }
  
}


export interface IAppUser {
	Username: string;
    DisplayName: string;
    Token: string;
    image?: string;
    Email?: string;
    PhoneNumber?: string;
    IsActive?:boolean;
}

export class AppUser implements IAppUser {
	Username: string = '';
    DisplayName: string= ''
    Token: string= ''
    image?: string= '';
    Email?: string= '';
    PhoneNumber?: string= '';
    IsActive?:boolean = false;
  
  constructor(init?: IAppUser) {
    Object.assign(this, init);
  }
  
}


export interface IAppConfig {
	Id: number
	Title: string
	Order:number
	// Type: number
	ConfigTypeId : number;	
	ConfigType: string;
	Det1: string
	Det2: string
	Det3: string
	Det4: string
	Det5: string
	//defaultSort: string
}

export class AppConfig implements IAppConfig {
	Id: number = 0;
	Title: string = '';
	Order:number = 0;
	// Type: number = 0;
	ConfigTypeId: number = 0;
	ConfigType: string = '';
	Det1: string = '';
	Det2: string = '';
	Det3: string = '';
	Det4: string = '';
	Det5: string = '';
	//defaultSort: string= 'desc';
  
  constructor(init?: IAppConfig) {
    Object.assign(this, init);
  }
}



export interface IHistory {
	Id: number
	Action: string
	FromStage: number
	ToStage: number
	ActionBy: string
	DateTime: Date
	Comment: string
	Details1: string
	Details2: string
	Details3: string
	Details4: string
	Details5: string
}

export class History implements IHistory {
	Id: number = 0;
	Action: string = '';
	FromStage: number = 0;
	ToStage: number = 0;
	ActionBy: string = "";
	DateTime: Date = new Date() ;
	Comment: string = '';
	Details1: string = '';
	Details2: string = '';
	Details3: string = '';
	Details4: string = '';
	Details5: string = '';
  
  constructor(init?: IHistory) {
    Object.assign(this, init);
  }
}

//==============================================

const IAppApiAPI = "/AppApi";

const DBFun = { 
  
  ExecuteQuery: (action: IApiAction) => agent.requests.post(`${IAppApiAPI}/Query`, action),    
  FileDownload: (action: IApiAction) => agent.requests.downloadPost(`${IAppApiAPI}/Attachment`, action),

  AppAllActionList: ()  =>  agent.requests.get(`${IAppApiAPI}/AppAllActionList`),	
  Execute: (action: FormData) => agent.requests.postForm(`${IAppApiAPI}/TakeAction`, action),  
  
  ActionList: (FlowId: number, Id: number, tableName: string, flowName: string) =>  agent.requests.get(`${IAppApiAPI}/ActionList/${FlowId}?itemId=${Id}&tableName=${tableName}&flowName=${flowName}&`),
  StatusList: (TableName: string)  =>  agent.requests.get(`${IAppApiAPI}/GetStatusList/${TableName}`),
  ConfigList: (configType: string)  =>  agent.requests.get(`${IAppApiAPI}/GetConfigList/${configType}`),
  UserList: ()  =>  agent.requests.get(`${IAppApiAPI}/GetUserList`),
  RoleList: ()  =>  agent.requests.get(`${IAppApiAPI}/GetRoleList`),
  
  list: (): Promise<any[]> => agent.requests.get(IAppApiAPI),
  details: (Id: number) => agent.requests.get(`${IAppApiAPI}/${Id}`),
  create: (item: any) => agent.requests.post(IAppApiAPI, item),
  update: (item: any) => agent.requests.put(`${IAppApiAPI}/${item.Id}`, item),
  delete: (Id: number) => agent.requests.del(`${IAppApiAPI}/${Id}`),
};

export default class ApiImpl {	
	AllActionList : IApiAction[] = [];
	configList: IAppConfig[] = [];
	roleList: IAppUserRoleMaster[] = [];
	userList: IAppUser[] = [];
	constructor() {
		makeObservable(this, {
			AllActionList: observable,
			configList: observable,	
			roleList: observable,	
			userList: observable,	
		});
	}

	updateAllActionList = async () => {
		this.AllActionList = await DBFun.AppAllActionList(); 
	}
	
	rolesFromArray(rolesList: AppUserRoleMaster[], strRoleArray: string) : AppUserRoleMaster[] {

		let roles : AppUserRoleMaster[] = [];
		if(strRoleArray && rolesList && rolesList.length > 0){
			strRoleArray.split(',').forEach( r => {
				let role = new AppUserRoleMaster();
				role.Id = r;
				var RoleName = rolesList.find(x => x.Id === r)?.Name;
				role.Name = RoleName ? RoleName : '' ;
				roles.push(role);
			});
		}
		
		return roles;			
	}

	rolesName(rolesList: AppUserRoleMaster[], strRoleArray:string):string{
		let strRoleNames = '';

		if(strRoleArray && rolesList && rolesList.length > 0){
			strRoleArray.split(',').forEach( r => {
				var RoleName = rolesList.find(x => x.Id === r)?.Name;
				if(strRoleNames){
					strRoleNames += ", "
				}
				strRoleNames += RoleName;				
			});
		}

		return strRoleNames;
	}
	
	getStatusList = async (TableName: string) => {		
		try {      		 
			return await DBFun.StatusList(TableName); 		  
		} catch (error) {
			return error;
		}
	}

	getUserList = async () => {		

		if(this.userList.length === 0){
			try {      		 
				this.userList =  await DBFun.UserList(); 		  
			} catch (error) {
				return error;
			}
		}									
		return this.userList;		
	}
	userFromArray(list: AppUser[], strArray: string) : AppUser[] {

		let objLst : AppUser[] = [];
		if(strArray && list && list.length > 0){
			strArray.split(',').forEach( r => {
				let obj = new AppUser();
				obj.Username = r;
				var name = list.find(x => x.Username === r)?.Username;
				obj.Username = name ? name : '' ;
				objLst.push(obj);
			});
		}
		
		return objLst;			
	}

	getRoleList = async () => {		
		//debugger;
		if(this.roleList.length === 0){
			try {      		 
				this.roleList =  await DBFun.RoleList(); 		  
			} catch (error) {
				return error;
			}
		}								
		return this.roleList;
		
	}

	getConfigList = async (configType: string, setData:any) => {
		//debugger;		
		try {      		 			
			var res = this.configList.filter(  x => x.ConfigType === configType )

			if(res.length === 0){
				res = await DBFun.ConfigList(configType)
				this.configList = [...this.configList, ...res]
			}						
			setData(res);
			return res;
		} catch (error) {
			return error;
		}
	}
	configFromArray(list: AppConfig[], strArray: string) : AppConfig[] {

		let objLst : AppConfig[] = [];
		if(strArray && list && list.length > 0){
			strArray.split(',').forEach( r => {
				let obj = new AppConfig();
				obj.Id = Number(r);
				var name = list.find(x => x.Id === Number(r))?.Title;
				obj.Title = name ? name : '' ;
				objLst.push(obj);
			});
		}
		
		return objLst;			
	}

	updateAttachments(formData: FormData, attachList: Attachment[]) : FormData {
		
		attachList.forEach( (f:Attachment) => {
            if( f.file){
                formData.append('FileList', f.file, f.FilePath);
            }            
        });
		return formData;					
	}
	
	updateActions = async (id: number, tableName: string, flowName: string, setActions: any, setError: any ) => {			
		try {  
			await DBFun.ActionList(0, id, tableName, flowName).then( (res) => {             
				if((res as any).errors){          
					setError((res as any).errors.Error);                        
				}
				else{
					setActions(res);
				}				
			});
		} catch (error) {
			setError("Problem in updateActions."); 			
		}
	}

	getActions = async (flowId: number, id: number, tableName: string, flowName: string) => {		
		try {      		 
			return await DBFun.ActionList(flowId, id, tableName, flowName); 		  
		} catch (error) {
			return error;
		}
	}

	getActionsByTitle = async (id: number, tableName: string, flowName: string) => {		

		if(this.AllActionList.length == 0)
			this.AllActionList = await DBFun.AppAllActionList(); 

		

		var flowId = 0;
		try {      		 
			return await DBFun.ActionList(flowId, id, '', ''); 		  
		} catch (error) {
			return error;
		}
	}
	//(): Promise<bool>

	LoadItem = async (ActionUniqName: string, ItemId: string, setActions: any, setItem: any, setError: any ) : Promise<any> => {	
		
		let act: AppApiAction = new AppApiAction()
        //act.ActionId =ActionId;   
		act.ActionUniqName =ActionUniqName;   
		act.Parm1 = ItemId;
		act.ReturnActions = true;
		var ret = null;
		try {        			 
			await DBFun.ExecuteQuery(act).then((res) => {  
				if((res as any).errors){          
					setError((res as any).errors.Error); 										
					ret = res;                   
				}
				else{
					if(res.Actions){
						setActions(res.Actions)
					}
					
					if(res.Actions[0]){
						ret = res.Result1[0];
						setItem(ret);						
					}
					else{
						setError("No Data");
						ret =  false;
					}															
				}				                        
			});  		   
		  } catch (error) {
			setError("error in LoadItem");
			ret =  false;			   
		}	
		return ret;
	}


	LoadDataList = async (ActionUniqName: string, setData:any, setLoading: any, setError: any ) => {	
		
		let act: AppApiAction = new AppApiAction()
        //act.ActionId =ActionId;
		act.ActionUniqName =ActionUniqName;    
		
		try {        			 
			await DBFun.ExecuteQuery(act).then((res) => {  
				if((res as any).errors ){          
					setError((res as any).errors.Error); 
					setLoading(false);                       
				}
				else{
					if(res.Result1){
						setData(res.Result1); 
					}
					else{
						setError("NoData");
					}					
					setLoading(false); 
				}                              
			});  		   
		  } catch (error) {
			setError("error in LoadData"); 
		  }		
	}

	ExecuteQuery = async (action: IApiAction) => {		
		try {        			 
		  return await DBFun.ExecuteQuery(action);  		   
		} catch (error) {
			return error;
		}
	}
	


	ExecuteAction = async (action: FormData, setError:any) => {
		var ret = null;		    		
		try {    			
			await DBFun.Execute(action).then((res) => {  
				if((res as any).errors){          
					setError((res as any).errors.Error); 	
					ret = false;				                
				}
				else{
					ret = res; 
				}                              
			});  				
		} catch (error) {
			setError("error in ExecuteAction"); ;
			ret = false;
		}
		return ret;
	}

	FileDownload = async (action: IApiAction) => {		
		try {        
		  debugger;		 
		  var itm = await DBFun.FileDownload(action);  		  
		  return itm;		 
		} catch (error) {
		 return error;
		}
	  }
}

export const ApiContext = createContext(new ApiImpl());

