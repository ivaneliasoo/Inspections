import { DateTime } from 'luxon'

const useDateTime = () => {
  const parseDate = (date: string) => {
    return DateTime.fromISO(date).toLocaleString()
  }

  const formatDate = (
    date: string,
    format: string = 'yyyy-MM-dd HH:mm'
  ): string => {
    return DateTime.fromISO(date).toFormat(format)
  }

  return { parseDate, formatDate }
}

export default useDateTime
