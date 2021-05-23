# Technical documentation 

## Main components

### Main Activity

In MainActivity.cs there is only one class with the same name which task is to provide functionality for the Main Menu and then delegate next tasks to other components.
It initializes the app with the needed GUI or it can restart it.

### Fragments

### Language

This part consists mainly of the class English which task is to load the available list of words (support for Slovak and Czech) and their translations in English. In the future there is a possibility to add new foreign language except of English.

### Database

Conceptually this is the part which is responsible for accessing the local database which is represented by Result class. ResultsDatabase class implements methods that work directly with the database, e.g. adding an element, removing it etc. 
The most complex class is DatabaseFileManager where serialization of the database and its deserialization from a file is implemented. There is a runtime check for accessing the storage data from the user (method CheckPermission). Then the file is copied tom Downloads folder (either in internal or external storage) and from there it can be accessed by the app in a new or an old phone. 

