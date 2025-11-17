# GitHub Actions Workflows for VotingApp

This directory contains CI/CD workflows for the VotingApp project.

## Workflows Overview

### 🔨 CI - Build and Test (`ci.yml`)
**Triggers:** Push and PR to master/main/develop branches, manual dispatch

**Jobs:**
- **build-and-test**: Builds the solution and runs unit tests
  - Restores dependencies
  - Builds in Release configuration
  - Runs unit tests with trx reporting
  - Uploads test results as artifacts
  
- **code-quality**: Analyzes code quality and coverage
  - Runs code coverage analysis
  - Generates coverage reports
  - Adds coverage comments to PRs
  
- **security-scan**: Scans for security vulnerabilities
  - Checks for vulnerable dependencies
  - Uploads security scan results

### 🚀 CD - Deploy to Azure (`cd.yml`)
**Triggers:** Push to master/main, manual dispatch

**Jobs:**
- **build**: Builds and publishes the application
- **deploy-production**: Deploys directly to production environment
  - Automatically deploys on push to master/main
  - Requires manual approval via GitHub environment protection
  - Creates release tags
  - Runs smoke tests

**Configuration Required:**
- Set `AZURE_WEBAPP_NAME` in environment variables
- Add GitHub secret:
  - `AZURE_WEBAPP_PUBLISH_PROFILE_PRODUCTION`

### ✅ PR Validation (`pr-validation.yml`)
**Triggers:** Pull request opened, synchronized, or reopened

**Jobs:**
- **validate-pr**: Validates pull request changes
  - Builds and tests the code
  - Checks code formatting
  - Comments on PR with validation results
  
- **pr-labeler**: Automatically labels PRs based on changed files

### 🌙 Nightly Build (`nightly.yml`)
**Triggers:** Daily at 2 AM UTC, manual dispatch

**Jobs:**
- **nightly-build**: Performs comprehensive nightly testing
  - Runs all tests with detailed output
  - Creates nightly build artifacts
  - Creates GitHub issue on failure

### 📦 Release (`release.yml`)
**Triggers:** Push tags matching `v*.*.*`, manual dispatch

**Jobs:**
- **create-release**: Creates GitHub releases
  - Builds self-contained executables for Windows and Linux
  - Generates changelog from git commits
  - Creates GitHub release with artifacts
  - Supports manual release creation with custom version

## Setup Instructions

### 1. Configure Azure Deployment (Optional)

If deploying to Azure:

1. Create Azure Web App for production
2. Download publish profile from Azure Portal
3. Add as GitHub secret:
   ```
   Settings → Secrets and variables → Actions → New repository secret
   ```
   - Name: `AZURE_WEBAPP_PUBLISH_PROFILE_PRODUCTION`
   - Value: [paste production publish profile XML]

4. Update `AZURE_WEBAPP_NAME` in `cd.yml`

### 2. Configure Branch Protection

Recommended branch protection rules for `master`/`main`:

- ✅ Require pull request before merging
- ✅ Require status checks to pass: `Build and Test`, `Code Quality Analysis`
- ✅ Require conversation resolution before merging
- ✅ Require linear history

### 3. Enable GitHub Environments

Create environment for deployment approvals:

1. Go to `Settings → Environments`
2. Create `production` environment
3. Add required reviewers for production approvals

## Local Testing

Test workflows locally using [act](https://github.com/nektos/act):

```bash
# Install act
choco install act-cli

# Test CI workflow
act push -W .github/workflows/ci.yml

# Test PR validation
act pull_request -W .github/workflows/pr-validation.yml
```

## Workflow Badges

Add these badges to your README.md:

```markdown
![CI](https://github.com/suhasaraos/SimpleVotingApp/workflows/CI%20-%20Build%20and%20Test/badge.svg)
![CD](https://github.com/suhasaraos/SimpleVotingApp/workflows/CD%20-%20Deploy%20to%20Azure/badge.svg)
[![Release](https://github.com/suhasaraos/SimpleVotingApp/workflows/Release/badge.svg)](https://github.com/suhasaraos/SimpleVotingApp/releases)
```

## Manual Workflow Execution

### Deploy to Production
1. Go to `Actions → CD - Deploy to Azure`
2. Click `Run workflow`
3. Click `Run workflow` to confirm

### Create Release
1. Go to `Actions → Release`
2. Click `Run workflow`
3. Enter version (e.g., v1.0.0)
4. Click `Run workflow`

## Troubleshooting

### Build Failures
- Check .NET version compatibility
- Ensure all NuGet packages are restored
- Review build logs for specific errors

### Test Failures
- Review test output in artifacts
- Check for environment-specific issues
- Ensure tests are isolated and repeatable

### Deployment Failures
- Verify Azure credentials are current
- Check Azure Web App configuration
- Review deployment logs in Azure Portal

## Best Practices

1. **Keep workflows fast**: Cache dependencies, run tests in parallel
2. **Fail fast**: Run quick checks (linting, formatting) before expensive operations
3. **Use artifacts**: Share build outputs between jobs
4. **Secure secrets**: Never log or expose secrets
5. **Monitor workflow usage**: Track Actions minutes and optimize as needed

## Additional Resources

- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [Azure Web Apps Deployment](https://docs.microsoft.com/en-us/azure/app-service/)
- [.NET CLI Documentation](https://docs.microsoft.com/en-us/dotnet/core/tools/)
