source ../.env
rm *.pem

# 1. Generate CA's private key and self-signed certificate
openssl req -x509 -newkey rsa:4096 -days 365 -nodes -keyout ca-key.pem -out ca-cert.pem \
  -subj "/C=${CERTS_COUNTRY}/ST=${CERTS_STATE}/L=${CERTS_LOCATION}/O=${CERTS_ORGANIZATION}/OU=Certificate Authority/CN=${CERTS_COMMON_NAME}/emailAddress=${CERTS_EMAIL}"

echo "CA's self-signed certificate"
openssl x509 -in ca-cert.pem -noout -text

# 2. Generate heat pump service's private key and certificate signing request (CSR)
openssl req -newkey rsa:4096 -nodes -keyout heat-pump-service-key.pem -out heat-pump-service-req.pem \
  -subj "/C=${CERTS_COUNTRY}/ST=${CERTS_STATE}/L=${CERTS_LOCATION}/O=${CERTS_ORGANIZATION}/OU=Heat Pump Service/CN=${CERTS_COMMON_NAME}/emailAddress=${CERTS_EMAIL}"

# 3. Use CA's private key to heat pump service's CSR
openssl x509 -req -in heat-pump-service-req.pem -days 365 -CA ca-cert.pem -CAkey ca-key.pem -CAcreateserial \
  -out heat-pump-service-cert.pem -extfile <(printf "subjectAltName=DNS:host.docker.internal")

echo "Signed certificate of Heat Pump Service"
openssl x509 -in heat-pump-service-cert.pem -noout -text

# 4. Generate heating gateway's private key and certificate signing request (CSR)
openssl req -newkey rsa:4096 -nodes -keyout heating-gateway-key.pem -out heating-gateway-req.pem \
  -subj "/C=${CERTS_COUNTRY}/ST=${CERTS_STATE}/L=${CERTS_LOCATION}/O=${CERTS_ORGANIZATION}/OU=Heating Gateway/CN=${CERTS_COMMON_NAME}/emailAddress=${CERTS_EMAIL}"

# 5. Use CA's private key to sign heating gateway's CSR
openssl x509 -req -in heating-gateway-req.pem -days 365 -CA ca-cert.pem -CAkey ca-key.pem -CAcreateserial \
  -out heating-gateway-cert.pem -extfile <(printf "subjectAltName=DNS:host.docker.internal")

echo "Signed certificate of Heating Gateway"
openssl x509 -in heating-gateway-cert.pem -noout -text
