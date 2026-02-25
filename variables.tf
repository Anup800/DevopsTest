variable "environment" {
  description = "Deployment environment name (for example: dev, prod)."
  type        = string
}

variable "location" {
  description = "Azure region for resources."
  type        = string
  default     = "eastus"
}

variable "resource_group_name" {
  description = "Name of the resource group for the Function App stack."
  type        = string
}

variable "function_app_name" {
  description = "Globally unique Azure Function App name."
  type        = string
}

variable "service_plan_name" {
  description = "Name of the App Service plan for the Function App."
  type        = string
}

variable "storage_account_name" {
  description = "Globally unique storage account name (3-24 lowercase alphanumeric chars)."
  type        = string
}

variable "apim_name" {
  description = "API Management instance name."
  type        = string
}

variable "apim_publisher_name" {
  description = "Publisher name for API Management."
  type        = string
}

variable "apim_publisher_email" {
  description = "Publisher email for API Management."
  type        = string
}

variable "tags" {
  description = "Tags applied to all resources."
  type        = map(string)
  default     = {}
}
