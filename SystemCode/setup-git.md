```bash
kenly-macbookpro2:ISS-IS02PT kenly$ git config --global user.name "kenly.ldk"
kenly-macbookpro2:ISS-IS02PT kenly$ git config --global user.email "kenly.ldk@gmail.com"
```

Create public/private key pair
```bash
kenly-macbookpro2:ISS-IS02PT kenly$ ssh-keygen -t rsa -C "kenly.ldk@gmail.com"
> Generating public/private rsa key pair.
Enter file in which to save the key (/Users/kenly/.ssh/id_rsa): kenly.ldk@gmail.com
Enter passphrase (empty for no passphrase): 
Enter same passphrase again: 
Your identification has been saved in kenly.ldk@gmail.com.
Your public key has been saved in kenly.ldk@gmail.com.pub.
The key fingerprint is:
SHA256:jDY5s+9XsfvjQWwuvZlZYiJCNVjH3jPPHKT/GZmXUxE kenly.ldk@gmail.com
The key's randomart image is:
+---[RSA 2048]----+
|          ...  E.|
|         o ..  o |
|        . o. .o .|
|       + . .oo+..|
|      B S    o=**|
|     . *    o= B=|
|      . . ..o.* B|
|       . ....+.X.|
|       .o.   oB. |
+----[SHA256]-----+
kenly-macbookpro2:ISS-IS02PT kenly$ 
```
