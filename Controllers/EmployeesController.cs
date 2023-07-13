using ASPNETMVC_CRUD.Data;
using ASPNETMVC_CRUD.Models;
using ASPNETMVC_CRUD.Models.DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETMVC_CRUD.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly MVCDemoDbContext mvcDemoDbContext;


        //ctor kısayol
        public EmployeesController(MVCDemoDbContext mvcDemoDbContext)
        {
            this.mvcDemoDbContext = mvcDemoDbContext;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var employees = await mvcDemoDbContext.Employees.ToListAsync();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddEmployeeViewModel addEmployeeRequest)
        {
            var employee = new Employee()
            {
                Id = Guid.NewGuid(),
                Name = addEmployeeRequest.Name,
                Email = addEmployeeRequest.Email,
                Salary = addEmployeeRequest.Salary,
                DateOfBirth = addEmployeeRequest.DateOfBirth,
                Department = addEmployeeRequest.Department
            };

            await mvcDemoDbContext.Employees.AddAsync(employee);
            await mvcDemoDbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> View(Guid id)
        {
            var employee = await mvcDemoDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if (employee != null) 
            {
				var viewModel = new UpdateEmployeeViewmodel()
				{
					Id = employee.Id,
					Name = employee.Name,
					Email = employee.Email,
					Salary = employee.Salary,
					DateOfBirth = employee.DateOfBirth,
					Department = employee.Department
				};
				return await Task.Run(() => View("View", viewModel));
			}

            return RedirectToAction("Index");  
        }


        [HttpPost]

        public async Task<IActionResult> View(UpdateEmployeeViewmodel model)
        {
			var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Salary = model.Salary;
                employee.DateOfBirth = model.DateOfBirth;
                employee.Department = model.Department;

                await mvcDemoDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }else
            {
                return RedirectToAction("Index"); //Error page yapıp oraya yönlendirmek lazım normalde
            }
        }

        [HttpPost]

        public async Task<IActionResult> Delete(UpdateEmployeeViewmodel model)
        {
            var employee = await mvcDemoDbContext.Employees.FindAsync(model.Id);

            if (employee != null)
            {
                mvcDemoDbContext.Employees.Remove(employee);
                await mvcDemoDbContext.SaveChangesAsync();

                return RedirectToAction("Index");
            }else
            {
				return RedirectToAction("Index"); //Error page olması gerek
			}

        }

    }
}
