import { ScrollView, StyleSheet, Text, View } from 'react-native';
import React, { createRef, useState } from 'react';
import {
  Icon,
  Input,
  Layout,
  Tab,
  TabBar,
  TabView,
  useTheme,
} from '@ui-kitten/components';
import { FormDefinitionResponse } from '../services/api/models';
import { useReports } from '../hooks/useReports';
import { Formik, FormikProps } from 'formik';
import NumericPicker from './NumericPicker';

const DynamicForms = () => {
  const theme = useTheme();
  const [selectedTabIndex, setSelectedTabIndex] = useState(0);
  //   const shouldLoadComponent = (index) => index === selectedTabIndex;

  const { workingReport: report, saveSignature } = useReports();
  console.log({ forms: report.forms });
  const formRef = createRef<FormikProps<FormDefinitionResponse>>();

  const formatPickerValue = (value: number) => {
    if (!value) return [0, 0, 0];
    const temp = value.toString().padStart(3, '0');
    return [parseInt(temp[0]), parseInt(temp[1]), parseInt(temp[2])];
  };

  if (!report || !report.forms) {
    return <Text>Empty</Text>;
  }

  const saveReadings = async (values: FormDefinitionResponse) => {};

  return (
    <TabView
      selectedIndex={selectedTabIndex}
      onSelect={(index) => setSelectedTabIndex(index)}>
      {report.forms!.map((schema, index) => {
        const TabIcon = (props) => <Icon {...props} name={schema.icon ?? ''} />;

        const defaultValues = {}

        schema.fields!.fieldsDefinitions!.forEach(({...f}) => {
                Object.defineProperty(defaultValues, f.fieldName!, {
                    value: f.defaultValue,
                    writable: true,
                    enumerable: true,
                    configurable: true
                })
          });

        return (
          <Tab
            key={`${schema.name}_${index}`}
            title={schema.title ?? ''}
            icon={TabIcon}>
            <ScrollView style={{ height: '100%' }}>
              <Formik
                innerRef={formRef}
                initialValues={defaultValues}
                enableReinitialize
                onSubmit={saveReadings}>
                {({ values, setFieldValue }) => {
                  return schema.fields!.fieldsDefinitions!.map(
                    (field, index) => {
                      return (
                        <Layout key={`layout_${index}`}>
                          {/* <Text>{field.label}</Text> */}
                          {field.inputType === 'text' && (
                            <Input
                              value={field.defaultValue}
                              disabled={!field.enabled}
                            />
                          )}
                          {field.inputType === 'textarea' && (
                            <Input
                              multiline={true}
                              numberOfLines={5}
                              value={field.defaultValue}
                              disabled={!field.enabled}
                            />
                          )}
                          {field.inputType === 'number' &&
                            field.rollerOnMobile && (
                              <NumericPicker
                                preppendLabel={field.label!}
                                appendLabel={field.suffix!}
                                key={field.fieldName}
                                itemSelected={() => {}}
                                defaultValue={formatPickerValue(
                                  values.defaultValues,
                                )}
                              />
                            )}
                        </Layout>
                      );
                    },
                  );
                }}
              </Formik>
            </ScrollView>
          </Tab>
        );
      })}
    </TabView>
  );
};

export default DynamicForms;

const styles = StyleSheet.create({});
