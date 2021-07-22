import React, { useContext, useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { observer } from 'mobx-react-lite';
import { Card, LinearProgress } from '@material-ui/core';
import MaterialTable from 'material-table';
import { ApiContext, AppApiAction, AppUserRoleMaster, IAppStatusList } from '../Api/Api';
import { AppNavigation } from './Navigation';
import TableButton from '../../app/common/form/TableButton';
import ErrorMessage from '../../app/common/common/ErrorMessage';

const NavigationList: React.FC = () => {

    const [loading, setLoading] = useState(true);
    const [data, setData] = useState<AppNavigation[]>();
    const [stausList, setStausList] = useState<IAppStatusList[]>();
    const [roleList, setRoleList] = useState<AppUserRoleMaster[]>();
    const [error, setError] = useState('');

    const ApiStore = useContext(ApiContext);
    useEffect(() => {    
        debugger;
        setLoading(true); 

        ApiStore.getRoleList().then( res => {
            setRoleList(res);
        })

        ApiStore.getStatusList(20).then( res => {
            setStausList(res);
        })

        let act: AppApiAction = new AppApiAction()
        act.ActionId = 34;      
        ApiStore.ExecuteQuery(act).then( (res) => {  
            if((res as any).errors){          
                setError( error + ", " + (res as any).errors.Error); 
                setLoading(false);                       
            }
            else{
                setData(res.Result1); setLoading(false); 
            }                              
        });

    },[ApiStore, ApiStore.ExecuteQuery]);



    const TableColumns = [     
        {title: "Order", field: "Order", defaultSort: "asc"},
        { title: "Title", field: "Title", defaultSort: "asc",
          render :  (values: any) => { return <Link to={`/NavigationEdit/${values.Id}`} >{values.Title}</Link> }  },   
        { title: "StatusId", field: "StatusId",
          render : (values: any) => {  return stausList &&  (stausList as IAppStatusList[]).find( u => u.Id === Number(values.StatusId) )?.Title }
        },  
        {title: "Path", field: "Path"},
        {title: "Icon", field: "Icon"},
        {title: "UserAccessRoles", field: "UserAccessRoles",
         render : (values: any) => {  return ApiStore.rolesName(roleList as any, values.UserAccessRoles)   }},          
    ];

    const TableActions = [
            {          
                icon: (values: any) => { return <TableButton  label="Add New" path="/NavigationEdit" /> },
                tooltip: 'Add New',
                isFreeAction: true, 
                onClick: (event:any) =>{  },                                     
            }
        ]; 

    if(loading){
        return <LinearProgress color="secondary"  className="loaderStyle" />     
    }

    return(
        <React.Fragment>
       
        <ErrorMessage message={error} />                           
        { data &&          
            <MaterialTable                    
              title="Navigation List"
              data={data as AppNavigation[]}
              columns={TableColumns as any}
              actions={TableActions as any}
              options={{ sorting:true, search: true, paging: true, filtering: true, exportButton: true, pageSize:10,  tableLayout: "auto"}}/>
        }       
        </React.Fragment>
        
    )
}

export default observer(NavigationList);