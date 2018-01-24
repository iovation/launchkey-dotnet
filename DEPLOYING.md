# Build Service
This project contains an AppVeyor config file in [appveyor.yml](appveyor.yml). Every successful build produces a `.nupkg` file which can be deployed to nuget.org for publishing.

# Versioning
AppVeyor provides automated build numbers which are derived from the appveyor.yml file. This follows the format of `3.0.0.{build}`. For major releases, it is recommended to change that template in appveyor.yml to something else, such as `3.1.0.{build}`.

# Deploying to nuget.org
Once the AppVeyor build is complete, download the nupkg file from the 'Assets' tab of the build status. Upload to the appropriate nuget.org project. That's it.

