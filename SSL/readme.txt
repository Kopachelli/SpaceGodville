1. Go to sslforfree.com and generate a certificate
2. Start a console with administrator priviledges
3. Pack certificates into a pfx file:
4. RUN openssl pkcs12 -export -out {outputName}.pfx -inkey {privateKeyName}.key -in {certificateName}.crt -certfile {caBundleName}.crt
5. Import the pfx certificate into windows via MMC (from start menu)
6. Bind the certificate to a port:
7. RUN netsh http add sslcert ipport=0.0.0.0:443 appid={{applicationGuid}} certhash={certHash}
Example: netsh http add sslcert ipport=0.0.0.0:443 appid={b23634b8-c7df-4299-ad40-0a82103a96b4} certhash=f87e454d07de8e420623529886e96346db7d5338
8. Profit