cd ../ &&
docker build -t ses-apiservice:1.0.0 -f ./SES.ApiService.Dockerfile . &&
docker build -t ses-authorizationservice:1.0.0  -f ./SES.AuthorizationService.Dockerfile . &&
docker build -t ses-mockpaymentproviderservice:1.0.0  -f ./SES.MockPaymentProviderService.Dockerfile . &&
docker build -t ses-paymentsystemservice:1.0.0 -f ./SES.PaymentSystemService.Dockerfile . &&
docker build -t ses-stockexchangeservice:1.0.0 -f ./SES.StockExchangeService.Dockerfile . &&
docker build -t ses-stockinformationservice:1.0.0  -f ./SES.StockInformationService.Dockerfile . &&
docker build -t ses-stockownershipservice:1.0.0 -f ./SES.StockOwnershipService.Dockerfile . &&
docker build -t ses-stocktransactionservice:1.0.0 -f ./SES.StockTransactionService.Dockerfile . &&
docker build -t ses-usermanagementservice:1.0.0 -f ./SES.UserManagementService.Dockerfile . &&
echo "Done building." &&
#cd SES.Queue && kubectl create configmap config-map --from-file=definitions.json=./config/definitions.json
#&& kubectl create configmap rabbitmq-conf-map --from-file=rabbitmq.conf=./config/rabbitmq.conf
#&& cd ../
echo "Deploying..." &&
cd ./SES.Deployment &&
kubectl apply --server-side -f https://github.com/kedacore/keda/releases/download/v2.11.2/keda-2.11.2.yaml &&
kubectl replace -f deployment.yml