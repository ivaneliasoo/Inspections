import { useFormikContext } from "formik";
import React, { useCallback, useEffect, useState } from "react";
import debounce from 'just-debounce-it'
import { Text } from "@ui-kitten/components";
import moment from "moment";


export const AutoSave = ({ debounceMs }: any) => {
  const formik = useFormikContext();
  const [lastSaved, setLastSaved] = useState('');
  const debouncedSubmit = useCallback(
    debounce(
      () => {
        if(!formik.dirty) return
        formik.submitForm().then(() => setLastSaved(`Last Saved: ${moment(new Date()).format('DD/MM/YYYY HH:mm')}`))
      },
      debounceMs
    ),
    [debounceMs, formik.submitForm, formik.dirty]
  );

  useEffect(() => {
    debouncedSubmit();
  }, [debouncedSubmit, formik.values]);

  let result = '';

  if (!!formik.isSubmitting) {
    result = "saving...";
  } else if (Object.keys(formik.errors).length > 0) {
    result = `ERROR: ${JSON.stringify(formik.errors)}`;
  } else if (lastSaved !== null) {
    result = `${lastSaved}`;
  }
  return <><Text style={{color: 'green'}}>{result}</Text></>;
};