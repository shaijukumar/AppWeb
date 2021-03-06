import React, { useContext, useEffect, useState } from 'react';
import { observer } from 'mobx-react-lite';
import { Button, LinearProgress, TextField  } from '@material-ui/core';
import MaterialTable from 'material-table';
import { Link } from 'react-router-dom';
import XLSX from "xlsx";

import { ActionConfig, ApiContext } from '../Api/Api';
import TableButton from '../../app/common/form/TableButton';
import ErrorMessage from '../../app/common/common/ErrorMessage';
import { TestOne } from './TestOne';

const TestOneList: React.FC = () => {
    
  const [loading, setLoading] = useState(true);
  const [data, setData] = useState<TestOne[]>();
  const [error, setError] = useState('');

  const ApiStore = useContext(ApiContext);
      
  useEffect(() => {    
    setLoading(true); 
    ApiStore.LoadDataList("TestOneGetNavigationList", setData, setLoading, setError );    
  },[ApiStore, ApiStore.LoadDataList]);

  const TableColumns = [     
    // {title: "Id", field: "TableItemId", defaultSort: "asc"},
    // {title: "Name", field: "Name", render :  (values: any) => { return <Link to={`/TestOne/${values.Id}`} >{values.Name}</Link> }  },   
    // {title: "StatusId", field: "StatusId", render : (values: any) => {  return stausList &&  (stausList as IAppStatusList[]).find( u => u.Id === Number(values.StatusId) )?.Title }     },  
    // {title: "IsActive", field: "IsActive"},
    // {title: "DOB", field: "DOB", render : (values: any) => { return moment(values.DOB).format("DD-MMM-YYYY")  }  },
    // {title: "Country", field: "Country"},
    // {title: "Manager", field: "Manager"} 
    
    {title: 'Order', field: 'Order'}, 
		{title: 'Title', field: 'Title', render :  (values: any) => { return <Link to={`/TestOneEdit/${values.Id}`} >{values.Title}</Link> } }, 		
		{title: 'IsActive', field: 'IsActive'}, 
		{title: 'DOB', field: 'DOB', type: "date" }, 	
		{title: 'Country', field: 'Country'}, 
		{title: 'Salary', field: 'Salary'},        
    {title: 'TableItemId', field: 'TableItemId', 
    
    editComponent: (value: any, onChange: any,) => {
      debugger;
      return (
        <TextField
          onChange={e => onChange(e.target.value)}
          value={value.value}
          multiline
        />
      );
    }},    
  ];

  const TableActions = [
    {          
        icon: (values: any) => { return <TableButton  label="Add New" path="/TestOneEdit" /> },
        tooltip: 'Add New',
        isFreeAction: true, 
        onClick: (event:any) =>{  },                                     
    },
    {          
        icon: (values: any) => { return <TableButton  label="Refresh"  /> },
        tooltip: 'Refresh',
        isFreeAction: true, 
        onClick: (event:any) =>{  ApiStore.LoadDataList("TestOneGetNavigationList", setData, setLoading, setError ); },                                     
    },
    {          
        icon: (values: any) => { 
            return <Button  variant="contained" color="primary"  component="label" size="small" >
                        Upload Data<input type="file" multiple={false} onChange={ImportTable}  id="raised-button-file" style={{display: "none",}} />
                   </Button>  },
        tooltip: 'Import Table',
        isFreeAction: true,
        onClick: (event:any) =>{  },   
        iconProps: { style: { fontSize: "34px", color: "green", borderRadius:"0%  !important" , backgroundColor:'rosybrown' } },            
    },
  ];

  const ImportTable = (evt:any) => {
    // setError('');        
    var files = evt.target.files; // FileList object
    var xl2json = new ExcelToJSON();
    xl2json.parseExcel(files[0]);
    evt.target.value = null;  
  };


class ExcelToJSON {
   
    parseExcel = function(file:any) {
        
        var reader = new FileReader();

        reader.onload = function(e:any) {
            debugger;
            var data = e.target.result;
            var workbook = XLSX.read(data, {
                type: 'binary'
            });

            // workbook.SheetNames.forEach(function(sheetName) {
            //     var XL_row_object = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);                                                           
            //     (XL_row_object as AppNavigation[]).forEach(function (item:AppNavigation) {
            //         var v = item.Title;
            //     });   
            //    return;               
            // });
            let formData = new FormData();
            ApiStore.ExecuteAction(formData, setError);

            var XL_row_object = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);  
            // ( async() => { 
            //     await Promise.all(
            //         var v = item.Title;
            //         // formData.append('ActionId', actionId.toString() ); 
            //         // formData.append('Parm1', JSON.stringify(values) );
            //         // formData.append('ItemId',  values.Id );                      
            //     );                                                 
            // })();

            ( async() => { 

                var XL_row_object = XLSX.utils.sheet_to_json(workbook.Sheets[workbook.SheetNames[0]]);
                var items = XL_row_object as TestOne[];

                var item  = items[0];
                debugger;
                if(item.Title) {
                      let formData = new FormData();
                      formData.append('ActionId', "18" ); 
                      formData.append('Parm1', JSON.stringify(item) );  
                      ApiStore.ExecuteAction(formData, setError).then( (res) => {
                          console.log("res");
                      });
                    }      

                // await Promise.all(
                //     items.map( async (item:TestOne) => {   
                //       if(item.Title) {
                //         let formData = new FormData();
                //         formData.append('ActionId', "18" ); 
                //         formData.append('Parm1', JSON.stringify(item) );  
                //         ApiStore.ExecuteAction(formData, setError).then( (res) => {
                //             console.log("res");
                //         });
                //       }                                            
                //     })
                // );



                alert("Updated");

                
            })();

        }

        reader.onerror = function(ex) {
            console.log(ex);
        };
  
        reader.readAsBinaryString(file);
    }        
}     

  if(loading){
    return <LinearProgress color="secondary"  className="loaderStyle" />     
  }

      return (
        <React.Fragment>       
        <ErrorMessage message={error} />                           
        { data &&          
            <MaterialTable                    
              title="TestOne List"
              data={data as TestOne[]}
              columns={TableColumns as any}
              actions={TableActions as any}
              options={{ sorting:true, search: true, paging: true, filtering: true, exportButton: true, pageSize:100,  tableLayout: "auto"}}

              editable={{

                onRowAdd: newData => new Promise(resolve => { 
                  resolve(true);  
                }),

                onRowUpdate: (newData, oldData) =>
                new Promise((resolve, reject) => {                 
                  resolve(true);                   
                }),


                onRowDelete: (oldData:any) =>
                  new Promise((resolve, reject) => {
                    
                    setTimeout(() => {
                      debugger;
                        var itemId = Number((oldData as any).Id);                        
                        let formData = new FormData();
                        formData.append('ActionId', "21" ); 
                        formData.append('Parm1', JSON.stringify(oldData) );
                        formData.append('ItemId',  (oldData as any).Id );

                        ApiStore.ExecuteAction(formData, setError).then( (res) => {            
                            if(res){
                                ApiStore.LoadDataList("TestOneGetNavigationList", setData, setLoading, setError );                        
                            }           
                        });             
                      resolve(true);
                    }, 10)
                  }),

              }}

              // cellEditable={{
              //   onCellEditApproved: (newValue, oldValue, rowData, columnDef) => {
              //     debugger;       
              //     //let filedName : any =  "Title"; //columnDef.field == null ? columnDef.field as string : "Title" 
              //     return new Promise((resolve, reject) => {
                    
              //       (rowData as any)[(columnDef as any).title] = newValue;                                                                                               
              //       resolve();                                  
              //     });
              //   }
              // }}

              

              />

              
        }       
        </React.Fragment>
    )
}

export default observer(TestOneList);
