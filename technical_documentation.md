# Technical documentation 

## Main components

### Main Activity

There is MainActivity class which task is to provide functionality for the Main Menu and then delegate next tasks to other components.
It initializes the app with the needed GUI or it can restart it.

### Fragments

This is a big part of the app's functionality. Various fragments are called from MainActivity class to change view and they are then replaced by other new ones.
* Dialog fragments - It is is represented by GameDialog class which is responsible for displaying dialog window when user wants to end an undone quiz.
* Quiz fragments - These fragments are essential for the quiz functionality and displaying the results of previous quizes from local database (ScoresFragment, GameFragment and ResultsFragment classes).
* MainMenu fragments - The fragment classes in this section serve for navigating from the menu to other parts of the app. There are classes MainFragment, RecordingsFragment, SettingsFragment and QuizesFragment.

### Language

This part consists mainly of the class English which task is to load the available list of words (support for Slovak and Czech) and their translations in English. In the future there is a possibility to add new foreign language except of English that also implements ILanguage interface.

### Database

Conceptually this is the part which is responsible for accessing the local database which is represented by Result class. ResultsDatabase class implements methods that work directly with the database, e.g. adding an element, removing it etc. 
The most complex class is DatabaseFileManager where serialization of the database and its deserialization from a file is implemented. There is a runtime check for accessing the storage data from the user (method CheckPermission). Then the file is copied tom Downloads folder (either in internal or external storage) and from there it can be accessed by the app in your new or current phone. 

