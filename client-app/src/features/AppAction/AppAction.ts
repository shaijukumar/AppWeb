import { AppStatusList } from "../AppStatusList/AppStatusList";

export interface IAppAction {
	Id: number
	UniqName: string
	Order: number
	Action: string
	ActionType: string	
	FromStatusList : AppStatusList[];
	ToStatusId: number	
	FlowId: number
	InitStatus: boolean
	TableId: number
	WhenXml: string
	ActionXml: string
}

export class AppAction implements IAppAction {
	Id: number = 0;
	UniqName: string = '';
	Order: number = 0;
	Action: string = '';
	FromStatusList : AppStatusList[] = [];
	ActionType: string = '';
	ToStatusId: number = 0;
	FlowId: number = 0;	
	InitStatus: boolean = false;
	TableId: number = 0;
	WhenXml: string = '';
	ActionXml: string = '';
  
  constructor(init?: IAppAction) {
    Object.assign(this, init);
  }
}

export class AppExport	{
	UniqName: string = '';
	Order: number = 0;
	FlowName: string = '';
	ActionType: string = '';
	InitStatus: string = '';
	FromStatus : string = '';
	Action: string = '';		
	ToStatus: string = '';
	WhenXml: string = '';
	ActionXml: string = '';  
}
	
