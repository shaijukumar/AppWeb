export interface IAppNavigation {
	Id: number
	Title: string
	Path: string
	Icon: string
	Access: number
}

export class AppNavigation implements IAppNavigation {
	Id: number = 0;
	Title: string = '';
	Path: string = '';
	Icon: string = '';
	Access: number = 0;
  
  constructor(init?: IAppNavigation) {
    Object.assign(this, init);
  }
}
