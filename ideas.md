#Ideas for session
This will be a session demonstrating the use of the Microsoft Graph from a cross-platform mobile app. Focus will be on a couple of the APIs - Excel and Groups.

##Presentation
* Short introduction to Microsoft Graph
* Short discussion of cross-platform options
  - Cordova
  - Xamarin
* Adopt Xamarin
  - Discuss Forms vs Native UI
* Discuss Auth (ADAL MSAL)
* Discuss direct REST calls vs .NET SDK 
* Show architecture
###Demo
* Show scaffold solution
* Build shared and platform-specific auth
* Enumerate Groups
* Select a Group and display data from Excel
  - Values
  - Charts
* Add/update data and see it updated in real-time on the back-end

##Technology
* Restrict to Xamarin running either Forms or Native on iOS, Android and Windows (UWP)
* MVVM pattern using a framework to hide some of the complexity? OR Build from scratch and leave "enterprise-ready" as an exercise for the reader?
* Can a group have a tag so we can filter on matching groups in the mobile app?

##User Stories
* As a mobile user I want to log in with my O365 credentials
* As a mobile user I want to choose a group to work with 
* As a mobile user I want to view a dashboard of the current values
* As a mobile user I want to have the UI displayed in my language
* As an administrator I want to create a group
* As an administrator I want to adminster (add, remove) users in a group