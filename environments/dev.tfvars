environment          = "dev"
location             = "eastus"
resource_group_name  = "rg-func-basic-dev"
function_app_name    = "func-basic-mytest-dev-001"
service_plan_name    = "func-linux-plan-dev"
storage_account_name = "stfuncbasicdev001"
apim_name            = "apim-demo-anup-dev"
apim_publisher_name  = "Anup Singh"
apim_publisher_email = "anup@example.com"

tags = {
  environment = "dev"
  workload    = "function-app"
}
