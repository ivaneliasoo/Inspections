import { ScrollView, StyleSheet, View } from 'react-native';
import React, { createRef, useState } from 'react';
import {
  Icon,
  Input,
  Layout,
  Tab,
  Text,
  TabView,
  useTheme,
  Select,
  SelectItem,
  IndexPath,
} from '@ui-kitten/components';
import {
  DynamicFieldMetadata,
  FormDefinitionResponse,
} from '../services/api/models';
import { useReports } from '../hooks/useReports';
import { Formik, FormikProps } from 'formik';
import NumericPicker from './NumericPicker';
import NumericPicker2 from './NumericPicker2';
import { AutoSave } from './AutoSave';

const DynamicForms = () => {
  const theme = useTheme();
  const [selectedTabIndex, setSelectedTabIndex] = useState(0);
  //   const shouldLoadComponent = (index) => index === selectedTabIndex;

  const { workingReport: report, updateDynamicForm } = useReports();
  const formRef = createRef<FormikProps<FormDefinitionResponse>>();

  const formatPickerValue = (value: number) => {
    if (!value) return [0, 0, 0];
    const temp = value.toString().padStart(3, '0');
    return [parseInt(temp[0]), parseInt(temp[1]), parseInt(temp[2])];
  };

  if (!report || !report.forms) {
    return <Text>Empty</Text>;
  }

  return (
    <TabView
      selectedIndex={selectedTabIndex}
      onSelect={(index) => setSelectedTabIndex(index)}
      shouldLoadComponent={(index) => index === selectedTabIndex}>
      {report
        .forms!.filter((s) => s.enabled)
        .map((schema, index) => {
          // const TabIcon = (props) => (
          //   <Icon {...props} name={schema.icon ?? ''} />
          // );

          const sections = schema
            .fields!.fieldsDefinitions!.filter((s) => s.enabled)
            .sort((a, b) => a.order! - b.order!)
            .reduce((acc, value) => {
              if (!acc[value.sectionTitle!]) {
                acc[value.sectionTitle!] = [];
              }
              acc[value.sectionTitle!].push(value);
              return acc;
            }, {});

          const saveReadings = async (values: FormDefinitionResponse) => {
            updateDynamicForm(schema.id!, values);
          };

          const defaultValues = {};

          schema.fields!.fieldsDefinitions!.forEach(({ ...f }) => {
            Object.defineProperty(defaultValues, f.fieldName!, {
              value: f.defaultValue,
              writable: true,
              enumerable: true,
              configurable: true,
            });
          });
          return (
            <Tab key={`${schema.name}_${index}`} title={schema.title ?? ''}>
              <ScrollView style={{ height: '95%', backgroundColor: 'white' }}>
                <Formik
                  innerRef={formRef}
                  initialValues={schema.values ?? defaultValues}
                  enableReinitialize
                  onSubmit={saveReadings}>
                  {({ values, setFieldValue }) => {
                    return Object.keys(sections).map((section, index) => {
                      return (
                        <>
                          <View key={`Autosave_View_${index}`} style={{ alignSelf: 'center' }}>
                            <AutoSave debounceMs={600} />
                          </View>
                          <Text
                            key={`section_${section}`}
                            category={'s1'}
                            style={{ margin: 10, fontWeight: '700' }}>
                            {section}
                          </Text>
                          {sections[section]
                            .sort((f: any) => f.order)
                            .map((field: any, index: number) => {
                              return (
                                <View
                                  key={`layout_${index}`}
                                  style={{ marginVertical: 5 }}>
                                  {field.inputType === 'text' && (
                                    <Input
                                      key={`input_${index}`}
                                      value={values[field.fieldName!]}
                                      disabled={!field.enabled}
                                      onChangeText={(value) => { setFieldValue(field.fieldName!, value); }}
                                    />
                                  )}
                                  {field.inputType === 'textarea' && (
                                    <Input
                                      key={`input_${index}`}
                                      multiline={true}
                                      numberOfLines={5}
                                      value={values[field.fieldName!]}
                                      disabled={!field.enabled}
                                      onChangeText={(value) => { setFieldValue(field.fieldName!, value); }}
                                    />
                                  )}
                                  {(field.inputType === 'number' &&
                                    field.rollerOnMobile &&
                                    (field as DynamicFieldMetadata)
                                      .rollerDigits == 3) ? (
                                      <NumericPicker
                                        key={`picker_${index}`}
                                        preppendLabel={field.label!}
                                        appendLabel={field.suffix!}
                                        itemSelected={(value) =>
                                          setFieldValue(field.fieldName, value)
                                        }
                                        defaultValue={formatPickerValue(
                                          parseInt(
                                            values[field.fieldName!],
                                          ),
                                        )}
                                      />
                                    ): null}
                                  {(field.inputType === 'number' &&
                                    field.rollerOnMobile &&
                                    (field as DynamicFieldMetadata)
                                      .rollerDigits == 4) && (
                                      <NumericPicker2
                                        preppendLabel={field.label!}
                                        appendLabel={field.suffix!}
                                        key={field.fieldName}
                                        itemSelected={(value) =>
                                          setFieldValue(field.fieldName, value)
                                        }
                                        defaultValue={formatPickerValue(
                                          parseInt(
                                            defaultValues[field.fieldName!],
                                          ),
                                        )}
                                      />
                                    )}
                                    {
                                      field.inputType === 'select' && <Select label={field.label} value={field.selectOptions.split(',')[values[field.fieldName!]]} selectedIndex={new IndexPath(values[field.fieldName!] ?? 0)} onSelect={(index) => {setFieldValue(field.fieldName, index.row) }}>
                                        {field.selectOptions.split(',').map((option, index) => {
                                          return <SelectItem key={`option_${option}`} title={option} />
                                        })}
                                      </Select>
                                    }
                                    {
                                      (field.inputType === 'number' && !field.rollerOnMobile) && <Input maxLength={field.maxLength} keyboardType={'numeric'} label={field.label} value={values[field.fieldName!]} onChangeText={(value) => { setFieldValue(field.fieldName!, value); }} />
                                    }
                                </View>
                              );
                            })}
                        </>
                      );
                    });
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
