environment          = "prod"
location             = "eastus"
resource_group_name  = "rg-func-basic-prod"
function_app_name    = "func-basic-mytest-prod-001"
service_plan_name    = "func-linux-plan-prod"
storage_account_name = "stfuncbasicprod001"
apim_name            = "apim-demo-anup-prod"
apim_publisher_name  = "Anup Singh"
apim_publisher_email = "anup@example.com"

tags = {
  environment = "prod"
  workload    = "function-app"
}
