import iziToast from 'izitoast'
import 'izitoast/dist/css/iziToast.min.css'

export enum NotificationTypes {
  error = 'error',
  warn = 'warn',
  info = 'info',
  success = 'success',
}

export interface ResponseMessage {
  message: string
  type: NotificationTypes | string
}
const notificationErrorParser = function (
  defaultErrorMessage?: string,
  notFoundMessage?: string,
  error?: Error | any
) {
  let strMsg: string = ''

  const msgResp: ResponseMessage = {
    message: '',
    type: NotificationTypes.error,
  }
  if (error.response !== undefined) {
    if (error.response.status === 404 && error.response.data.title) {
      // verifico que title existe en data para identificar que no tiene mensaje de error desde el api si no que es solo un Not Found
      msgResp.message = notFoundMessage || 'no results'
      msgResp.type = NotificationTypes.info

      return msgResp
    } else if (error.response.status === 404 && !error.response.data.title) {
      msgResp.type = NotificationTypes.error
      msgResp.message = `${defaultErrorMessage} <br> ${notFoundMessage}`

      return msgResp
    } else {
      // if (error.response.status) {
      const { title, detail, errors } = error.response.data
      // Caso (1) cuando contenga errores "data.errors"
      // si el Api pasa por un "validation" devuelve propiedades de errores (no array),  estos se recorren y se extrae cada mensaje de error.
      if (errors !== undefined) {
        if (Array.isArray(errors)) {
          // Manejo Errores API segun estandard
          // RFC 7807 - Problem Details Standard for Http APIs https://tools.ietf.org/html/rfc7807
          strMsg += errors
            .map((err) => {
              return `<li>${err.message}</li>`
            })
            .join('')
        } else if (errors instanceof Object) {
          // Manejo Errores Validations del API la diferencia es que errors en este caso es un Object, y los errores son de tipo string y no otro Object
          if (errors['']) {
            strMsg += errors['']
              .map((err: any) => {
                return `<li>${err}</li>`
              })
              .join('')
          } else {
            const keys = Object.keys(errors)
            keys.forEach((key) => {
              strMsg += errors[key]
                .map((err: any) => {
                  return `<li>${err}</li>`
                })
                .join('')
            })
          }
        }
        // TODO: Falta el caso donde vienen mas de u field con mas de un error

        msgResp.message = `
        <details id=errorsContainer>
          <summary>${defaultErrorMessage}${
          title ? '<br>' + title : detail ? '<br>' + detail : ''
        }</summary>
          <ul id="details">
            ${strMsg}
          </ul>
        </details>`
      }
      // Comento este caso porque ahora todas las excepciones generadas por el api cumplen con el standard
      // RFC 7807 - Problem Details Standard for Http APIs https://tools.ietf.org/html/rfc7807

      //  else if (error.status === 500) {
      //   // Caso (2). cuando es solo una exception, solo se devuelve un mensaje en "data.message"
      //   var msg = error.response.detail
      //   msgResp.type = error.status === 500 ? 'error' : 'warn'

      //   // Reemplazamos los caracteres de "/a" que vienen en el mensaje. Esta forma fué que la encontré si hay otra luego se mejora.
      //   msgResp.message = '<br>' + msg.replace(/\/a/g, '')
      // }

      return msgResp
    }
  } else {
    // Caso (3). En el caso que response sea "undefined" pero si se tenga un request y un responseText !="", entonces tomo el mensaje del request.
    // ojo se que funciona este codigo porque lo saqué del bloque y lo probé aparte y extrae bien el mensaje.
    // sin embargo hasta el momento no se ha dado este caso, pero igual lo tenemos contemplado.

    if (error.request !== undefined && error.request.responseText !== '') {
      const msgRequest = JSON.parse(error.request.responseText)
      let msgReq = ''

      for (const msg in msgRequest) {
        // eslint-disable-next-line no-prototype-builtins
        if (msgRequest.hasOwnProperty(msg)) {
          // TODO: Corregir esto
          if (msg === 'message') {
            msgReq = `${msgReq}<br>${msgRequest[msg]}`
          }
        }
      }

      msgResp.message = msgReq
      return msgResp
    } else {
      // Se devuelvee el mensaje por defecto que se pasó desde la vista.
      msgResp.message = `<br>${defaultErrorMessage}<br>Detalle: ${error.message}`
    }

    return msgResp
  }
}

export interface Notify {
  title: string
  message?: string
  type: NotificationTypes
  notFoundMessage?: string
  error?: Error
}

export interface NotifyParams {
  title?: string
  message?: string
  type: NotificationTypes | string
  notFoundMessage?: string
  error?: Error | any
}

const notify: any = ({
  title,
  message,
  type,
  notFoundMessage,
  error,
}: NotifyParams): Notify => {
  if (error instanceof Object && error !== null) {
    const parsedError = notificationErrorParser(message, notFoundMessage, error)
    message = parsedError.message
    type = parsedError.type
  }

  if (type === NotificationTypes.warn) {
    type = 'warning'
  }

  return (iziToast as any)[type]({
    message,
    title,
    timeout: 2500,
    displayMode: 2,
    resetOnHover: true,
    position: 'topRight',
    layout: 2,
  })
}

const useNotifications = () => {
  return {
    notify,
    NotificationTypes,
    notificationErrorParser,
  }
}

export { useNotifications }
