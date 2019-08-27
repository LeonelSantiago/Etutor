# Etutor
Official repository for the web version of E-Tutor.

Here will be worked all the web based features of the product.

## Requeriments
- Visual Studio 2017
- .NET Core SDK 2.2.2 and 2.2.1 installed
- SQL Server


# Git workflow

## Al iniciar HU

- Crea una nueva rama
- git checkout -b primer-nombre-primer-apellido/numero-hu-tfs/descripción-corta development
- Ejemplo
git checkout -b juan-perez/14/eje-estrategico-admin development

## Mientras trabajas HU
 
- Si tienes cambios sin terminar (archivos cambiados, no has hecho commit)
- Guarda temporalmente tus cambios
- git stash
- Actualiza tu version local del repositorio con nuevos cambios del servidor
- git fetch
- Sincroniza tus cambios con la rama de desarrollo
- git rebase -p origin/development
- Obten tus cambios de vuelta
- git stash pop
- Si tus cambios estan guardados (les hiciste commit)
- Actualiza tu version local del repositorio con nuevos cambios del servidor
- git fetch
- Sincroniza tus cambios con la rama de desarrollo
- git rebase -p origin/development
- Si tus cambios tienen problemas con el git rebase
- Para detener cuando "pick" haya sido reemplazado por "edit" o cuando un comando falle debido a errores de merge y hayas resuelto el inconveniente.
- git rebase --continue
- Para guardar su confirmación, guarde y salga del archivo con el comando :wq
  
## Al finalizar HU
- Revisa las correciones de ESLint
- npm run lint
- Actualiza tu version local del repositorio con nuevos cambios del servidor
- git fetch
- Sincroniza tus cambios con la rama de desarrollo
- git rebase -p origin/development
- Sube tus cambios tu rama en el servidor
- git push -u origin NOMBRE-DE-TU-RAMA
- Entra a TFS y entra a la sección de "Pull Requests"
- Crea una nueva solicitud desde tu rama A LA RAMA DE DESARROLLO
- Espera a que tu solicitud sea aprobada (o denegada)
- Vuelve a la rama de desarrollo
- git checkout development
- Descarga los cambios de desarrollo
- git pull
- Borra tu rama local
- git branch -d NOMBRE-DE-TU-RAMA

## How to Commit
- Para normalizar los commits en el equipo de trabajo optamos por hacer lo siguiente:
- Se agregara un guion "-" antes de comenzar a escribir el comentario
- Por cada comentario un salto de linea y luego el guion"-" antes de comenzar
- Luego damos dos saltos de lineas y ponemos una cabezera llamada
- Related Work Items:

Luego de un salto de linea colocamos poniendo delante el simbolo de numero "#" los items relacionados
Ejemplo:
- Este es un comentario descriptivo de la pantalla equis que reslize
- Este otro comentario tambien es relevante y por eso esta aqui
- Si no soy un comentario relevante mejor ni me pongas 
- El mejor equipo del mundo es este
--------------------SALTO DE LINEA--------------------
Elementos de trabajo relacionados:
#13
#15
#19
 
 
## Comandos de utilidad
Para ver commits que no han sido subidos al repositorio: 
git log --branches --not --remotes
