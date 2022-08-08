import { useNotifications } from './use-notifications'

describe('Notifications parser test', () => {
  it('devuelve notFoundMessage y el tipo info cuando el error es 404', () => {
    const { notificationErrorParser } = useNotifications()
    const error = {
      response: {
        status: 404,
        data: { title: 'error error', detail: 'detail message' },
      },
    }
    const result = notificationErrorParser(
      'Mensaje por Defecto',
      'Mensaje cuando devuelve 404',
      error
    )
    expect(result.message).toContain('Mensaje cuando devuelve 404')
    expect(result.message).not.toContain('undefined')
    expect(result.type).toContain('info')
  })
})
