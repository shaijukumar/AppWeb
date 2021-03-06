
import { createContext } from "react";
import { observable, action, runInAction, makeObservable } from "mobx";
import agent from "../../app/api/agent";
import { AppAction, IAppAction } from "./AppAction";


const IAppActionAPI = "/AppAction";

const DBFun = {
  list: (): Promise<IAppAction[]> => agent.requests.get(IAppActionAPI),
  details: (Id: number) => agent.requests.get(`${IAppActionAPI}/Details/${Id}`),
  create: (item: IAppAction) => agent.requests.post(IAppActionAPI, item),
  update: (item: IAppAction) =>
    agent.requests.put(`${IAppActionAPI}/${item.Id}`, item),
  delete: (Id: number) => agent.requests.del(`${IAppActionAPI}/${Id}`),
  flowActions: (Id: number) => agent.requests.get(`${IAppActionAPI}/FlowActions/${Id}`),
};



export default class AppActionStoreImpl {

  loading = false;
  updating = false;
  itemList: IAppAction[] = [];
  flowList: IAppAction[] = [];
  item: AppAction = new AppAction()

 

  constructor() {
    makeObservable(this, {
         itemList: observable,
         flowList: observable,
         loading: observable,
         updating: observable,
         item: observable,
         getList: action,
         loadItem: action,
         editItem: action
    });
  }
 
  flowActions = async (id: number) => {        
    this.loading = true;
    try {               
      this.flowList = await DBFun.flowActions(id);       
      this.loading = false;         
      return this.flowList;          
    } catch (error) {
      runInAction( () => {
        this.loading = false;  
        //agent.requests.ErrorPage(error.message) ;            
        return error;
      });
    }
  }

  getList = async () => {        
    this.loading = true;
    try {               
      this.itemList = await DBFun.list();       
      this.loading = false;                   
    } catch (error) {
      runInAction( () => {
        this.loading = false;  
        agent.requests.ErrorPage(error.message) ;            
        throw error;

      });
    }
  }

  loadItem = async (id: number) => {
    this.loading = true;
    try {
      this.itemList = await DBFun.list(); 
      this.item = await DBFun.details(id); 
      
      this.loading = false;      
      return this.item;     
      } catch (error) {
        console.log(error);
        this.loading = false;
      }
  }

 editItem = async (item: IAppAction) => {    
    this.loading = true;
    try {        
      let itm = new  AppAction();
      if (item.Id) {
        itm = await DBFun.update(item);
      } else {
        itm = await DBFun.create(item);
      }
      this.loading = false;         
      return itm;   
    } catch (error) {
      this.loading = false;       
      return error;
    }
  };

  deleteItem = async (id: number) => {
    this.updating = true;
    this.loading = true;
    try {
      await DBFun.delete(id);    
      this.updating = false;   
      this.loading = false;
    } catch (error) {    
      this.updating = false;  
      this.loading = false;             
      console.log(error);
      throw error;
    }
  };  
}

export const AppActionContext = createContext(new AppActionStoreImpl());

