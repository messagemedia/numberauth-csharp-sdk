node {
    stage 'Checkout'
        checkout scm

    stage 'Prepare'
        bat 'nuget restore "NumberAuthorisation.sln"'
        bat 'nuget install NUnit.Runners -Version 3.2.1 -OutputDirectory testrunner'

    stage 'Build'
        bat "MSBuild.exe /p:Configuration=Release \"NumberAuthorisation.sln\""
        stage 'Test'
        bat "testrunner\\NUnit.ConsoleRunner.3.2.1\\tools\\nunit3-console.exe NumberAuthorisation.Tests\\bin\\Release\NumberAuthorisation.Tests.dll"            
     }