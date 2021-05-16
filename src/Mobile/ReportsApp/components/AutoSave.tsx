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
        console.log({ touched: formik.touched })
        formik.submitForm().then(() => setLastSaved((moment(new Date()).format('DD/MM/YYYY HH:mm'))))
      },
      debounceMs
    ),
    [debounceMs, formik.submitForm]
  );

  useEffect(() => {
    debouncedSubmit();
  }, [debouncedSubmit, formik.values]);

  let result = '';

  if (!!formik.isSubmitting) {
    result = "saving...";
  } else if (Object.keys(formik.errors).length > 0) {
    result = ''; // `ERROR: ${JSON.stringify(formik.errors)}`;
  } else if (lastSaved !== null) {
    result = `Last Saved: ${lastSaved}`;
  }
  return <><Text>{result}</Text></>;
};