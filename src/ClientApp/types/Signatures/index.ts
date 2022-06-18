import { ResponsibleType } from '~/services/api'

export * from './Models'
export * from './Commands'

export const responsibleTypesString: ({ id: string; text: any } | undefined)[] =
  Object.keys(ResponsibleType)
    .map((key: any) => {
      if (!isNaN(Number(key.toString()))) {
        return undefined
      }

      return { id: key, text: key }
    })
    .filter(i => i !== undefined)

export const responsibleTypes: ({ id: string; text: any } | undefined)[] =
  Object.keys(ResponsibleType)
    .map((key: any) => {
      if (!isNaN(Number(key.toString()))) {
        return undefined
      }

      return { id: ResponsibleType[key], text: key }
    })
    .filter(i => i !== undefined)
