using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using IOT_Integration_For_Vital_Signs_Monitoring_System.Models;
using System;
using IOT_Integration_For_Vital_Signs_Monitoring_System.Services;
using Microsoft.AspNetCore.Authorization;

namespace IOT_Integration_For_Vital_Signs_Monitoring_System.Controllers;
public class HomeController : Controller
{

    private readonly AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    //LIST: Display all records
    [Authorize]
    public IActionResult Index()
    {
        var patient = _context.Patient
            .OrderByDescending(p => p.UpdatedDate)
            .ToList();
        return View(patient);
    }

    // CREATE: Display form
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // CREATE: Save new record
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(Patients patient, IFormFile? ProfileImage)
    {
        if (ModelState.IsValid)
        {
            // Handle file upload if a new profile image is selected
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // Define the path to save the image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", fileName);

                // Save the new file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                // Update the profile image path in the database
                patient.ProfileImagePath = fileName;
            }
         
            _context.Patient.Add(patient);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(patient);
    }

    // EDIT: Display existing record in form
    [Authorize]
    public IActionResult Edit(int id)
    {
        var patient = _context.Patient.Find(id);
        if (patient == null)
        {
            return NotFound();
        }
        return View(patient);
    }

    // EDIT: Save updated record
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Edit(Patients patient, IFormFile? ProfileImage)
    {
        if (ModelState.IsValid)
        {
            // ✅ Step 1: Get the existing patient from the database
            var existingPatient = await _context.Patient.FindAsync(patient.Id);

            // ✅ Step 2: Update fields manually
            existingPatient.Name = patient.Name;
            existingPatient.LastName = patient.LastName;
            existingPatient.Gender = patient.Gender;
            existingPatient.Birthdate = patient.Birthdate;
            existingPatient.Address = patient.Address;

            // ✅ Step 3: Handle file upload
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                // ✅ Step 1: Delete old image if it exists
                if (!string.IsNullOrEmpty(existingPatient.ProfileImagePath))
                {
                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", existingPatient.ProfileImagePath);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // ✅ Step 2: Save the new image
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var newImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", fileName);

                using (var stream = new FileStream(newImagePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                // ✅ Step 3: Update the DB record
                existingPatient.ProfileImagePath = fileName;
            }


            //✅ Step 4: Save changes
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(patient);
    }


    // DELETE: Confirm delete
    [Authorize]
    public IActionResult Delete(int id)
    {
        var patient = _context.Patient.Find(id);
        if (patient == null)
        {
            return NotFound();
        }
        return View(patient);
    }

    // DELETE: Remove from database
    [Authorize]
    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var patient = _context.Patient.Find(id);
        if (patient != null)
        {
            var records = _context.Record
            .Where(r => r.Name == patient.Name && r.LastName == patient.LastName) // Filter only patient Name and LastName 
            .ToList();

            _context.Record.RemoveRange(records);
            _context.Patient.Remove(patient);
            _context.SaveChanges();
        }
        return RedirectToAction("Index");
    }

    //DETAILS: Display patient details and vital datas
    [Authorize]
    public IActionResult Details(int id)
    {
        var patient = _context.Patient.Find(id);
        if (patient == null)
        {
            return NotFound();
        }

        var records = _context.Record
        .Where(r => r.Name == patient.Name && r.LastName == patient.LastName) // Filter only patient Name and LastName 
        .OrderByDescending(r => r.UpdatedDate) //Ordered by Date and time
        .ToList();

        ViewBag.Records = records; // Pass records using ViewBag or create a ViewModel

        return View(patient);
    }

    // AddData: Display existing record in form
    [Authorize]
    public IActionResult AddData(int id)
    {
        var patient = _context.Patient.Find(id);
        if (patient == null)
        {
            return NotFound();
        }
        return View(patient);
    }

    // AddData: Save updated record
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddData(Patients patient, Records record, IFormFile? ProfileImage)
    {
        if (ModelState.IsValid)
        {
            // Re-fetch the existing patient from DB
            var existingPatient = await _context.Patient.FindAsync(patient.Id);
            if (existingPatient == null) return NotFound();

            // Update only fields you want to change
            existingPatient.Weight = patient.Weight;
            existingPatient.Height = patient.Height;
            existingPatient.Temperature = patient.Temperature;
            existingPatient.Systolic = patient.Systolic;
            existingPatient.Diastolic = patient.Diastolic;

            existingPatient.BloodPressure = $"{patient.Systolic} / {patient.Diastolic}";
            existingPatient.BMI = CalculateBMI.CaculatePatientBMI(patient.Weight, patient.Height);
            existingPatient.UpdatedDate = DateTime.Now;

            // DO NOT overwrite ProfileImagePath unless there's a new upload
            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/profiles", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(stream);
                }

                existingPatient.ProfileImagePath = fileName;
            }

            // Save to patient
            _context.Patient.Update(existingPatient);

            // Save to records
            record.Name = existingPatient.Name;
            record.LastName = existingPatient.LastName;
            record.Weight = existingPatient.Weight;
            record.Height = existingPatient.Height;
            record.Temperature = existingPatient.Temperature;
            record.Systolic = existingPatient.Systolic;
            record.Diastolic = existingPatient.Diastolic;
            record.BloodPressure = existingPatient.BloodPressure;
            record.BMI = existingPatient.BMI;
            _context.Record.Add(record);

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        return View(patient);
    }

}
