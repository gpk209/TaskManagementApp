export class Task{
    Id: Number = 0;
    Title: string="";
    Description: string="";
    Status: string = "Pending"; // Pending, Completed (backend enum)
    Priority: string = "Medium"; // Low, Medium, High (backend enum)
    isEditMode: boolean = false;
    DueDate: Date = new Date();
    
    constructor(
        id: Number, 
        title:string, 
        description:string, 
        status:string, 
        dueDate:Date,
        priority: string = "Medium"
    ){
        this.Id = id;
        this.Description = description;
        this.Title = title;
        this.Status = status;
        this.DueDate = dueDate;
        this.Priority = priority;
    }
}