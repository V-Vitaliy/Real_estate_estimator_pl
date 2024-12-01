# **Real Estate Price Prediction Project**

## **Project Description**

This project is designed to predict real estate prices and manage a local database of real estate data. The project consists of the following components:

1. **Jupyter Notebook**:  
   - Contains the code for training two machine learning models.  
   - These models are used to predict real estate prices based on provided data.  

2. **ConsoleApp.sln**:  
   - A console application written in C# that connects to a local Microsoft SQL Server database.  
   - It provides a simple interface for the following actions:  
     - Add new real estate records to the database.  
     - Predict the price of real estate using trained models.  
     - Show data.  

3. **Real Estate Data File**:  
   - A comprehensive dataset containing information about real estate in Krakow and Warsaw.  

---

## **Getting Started**

### **Prerequisites**

To run this project, you need the following:  
- Python 3.8 or higher with Jupyter Notebook installed.  
- Required Python libraries: `pandas`, `scikit-learn`, `numpy`, `matplotlib`, etc. (check `requirements.txt` for details).  
- Visual Studio or any compatible C# development environment to work with `ConsoleApp.sln`.  
- Microsoft SQL Server to host the real estate database.  

### **Installation**

1. **Set up the Python environment**:  
   - Install dependencies:  
     ```bash
     pip install -r requirements.txt
     ```  
   - Open the Jupyter Notebook and run the code to train and save the machine learning models.
   - or load fitted models from 'models'

2. **Set up the C# console application**:  
   - Open `ConsoleApp.sln` in Visual Studio.  
   - Configure the connection string in the application to point to your local SQL Server instance.  

3. **Set up the database**:  
   - Use the provided SQL script (`script_real_estate_1.sql`) to create the database structure.  
   - Populate the database with the real estate dataset.

---

## **Usage**

1. **Train or load the models**:  

2. **Run the console application**:  
   - Use the interface to perform the following actions:  
     - Add a new real estate record.  
     - Predict the price of a property.  
     -Show data in database.

3. **Analyze data**:  
   - Use the Jupyter Notebook to analyze and visualize real estate trends.  


