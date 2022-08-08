import { extend } from 'vee-validate'
import { required, email } from 'vee-validate/dist/rules'

extend('required', {
  ...required,
  message: 'Required Field',
})

extend('email', {
  ...email,
  message: 'Invalid Email',
})

extend('precedesDate', {
  validate: (dateTo, { dateFrom }: any) => {
    return dateTo >= dateFrom
  },
  message: 'Invalid Dates Range',
  params: [{ name: 'dateFrom', isTarget: true }],
})

extend('password', {
  params: [{ name: 'other', isTarget: true }],
  validate: (value, { other }: any) => {
    return value === other
  },
  message: 'Passwords do not match.',
})
