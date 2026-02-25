# Azure Function App + Terraform (Dev/Prod)

This repository now supports **two Terraform environments** (`dev` and `prod`) and a **GitHub Actions workflow** that:

1. Authenticates to Azure using OIDC.
2. Selects the target Terraform environment.
3. Runs `terraform init/validate/plan/apply`.
4. Builds and publishes the .NET isolated Azure Function App.
5. Deploys the Function App package to Azure.

## Terraform environments

Environment variable files:

- `environments/dev.tfvars`
- `environments/prod.tfvars`

Use Terraform workspaces + tfvars to deploy either environment:

```bash
terraform init
terraform workspace select dev || terraform workspace new dev
terraform plan -var-file=environments/dev.tfvars
terraform apply -var-file=environments/dev.tfvars
```

For prod:

```bash
terraform workspace select prod || terraform workspace new prod
terraform plan -var-file=environments/prod.tfvars
terraform apply -var-file=environments/prod.tfvars
```

## GitHub Actions workflow

Workflow file: `.github/workflows/deploy-functionapp.yml`

### Triggers

- Push to `dev` branch -> deploys **dev**.
- Push to `main` branch -> deploys **prod**.
- Manual dispatch -> choose `dev` or `prod`.

### Required GitHub secrets

Configure these repository (or environment) secrets:

- `AZURE_CLIENT_ID`
- `AZURE_TENANT_ID`
- `AZURE_SUBSCRIPTION_ID`

These are used by `azure/login@v2` with federated identity (OIDC).

## Notes

- Function App, APIM, service plan, and storage account names are now provided by environment-specific tfvars.
- Storage account names must be globally unique in Azure.
- Consider setting branch protections and required reviewers for the `prod` environment in GitHub.
