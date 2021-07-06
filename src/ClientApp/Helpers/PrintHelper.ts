import moment from 'moment'
import pdfMake from 'pdfmake/build/pdfmake'
import pdfFonts from 'pdfmake/build/vfs_fonts'
import { TDocumentDefinitions } from 'pdfmake/interfaces'
import { ReportQueryResult, SignatureQueryResult, CheckListQueryResult, ResponsibleType, CheckValue } from '../services/api'

export class PrintHelper {
  private readonly store: any
  constructor (store: any) {
    this.store = store
  }

  async getReport (id: number): Promise<ReportQueryResult> {
    const report: ReportQueryResult = await this.store.dispatch(
      'reportstrore/getReportById',
      id,
      { root: true }
    )
    return report
  }

  async getReportPhotos (id: number): Promise<any> {
    const photos: any = await this.store.dispatch(
      'reportstrore/getReportPhotos',
      id,
      { root: true }
    )
    return photos
  }

  getImage (url: string) {
    return new Promise((resolve, reject) => {
      const img = new Image()
      img.setAttribute('crossOrigin', 'anonymous')

      img.onload = () => {
        const canvas = document.createElement('canvas')
        canvas.width = img.width
        canvas.height = img.height

        const ctx = canvas.getContext('2d')
        ctx!.drawImage(img, 0, 0)

        const dataURL = canvas.toDataURL('image/png')

        resolve(dataURL)
      }

      img.onerror = (error) => {
        reject(error)
      }

      img.src = url
    })
  }

  public async print (reportId: number) {
    const report = await this.getReport(reportId);

    (window as any).pdfMake.vfs = pdfFonts.pdfMake.vfs

    const docDef: TDocumentDefinitions = {
      header: [{
        text: report.title!,
        margin: [0, 15, 30, 0],
        style: 'name'
      },
      {
        text: report.formName!,
        margin: [0, 0, 30, 0],
        style: 'subtitle'
      }],
      content: [
        {
          text: 'PARTICULARS OF INSTALLATION',
          style: 'title'
        },
        {
          text: `Name of Installation: ${report.name!.toUpperCase()} Approved Load: ${report.licenseAmp}A/${report.licenseVolt}V`,
          style: 'fieldName'
        },
        {
          text: `Address of Installation: ${report.address ? report.address.toUpperCase() : ''} (${report.licenseAmp} kVA)`,
          style: 'fieldName'
        },
        {
          text: `Electrical Installation License No: ${report.licenseNumber} Date & Time of Inspection: ${moment(report.date).format('DD/MM/YYYY')}`,
          style: 'fieldName'
        }
      ],
      styles: {
        name: {
          fontSize: 16,
          bold: true,
          alignment: 'right'

        },
        subtitle: {
          fontSize: 12,
          decoration: 'underline',
          alignment: 'right'
        },
        title: {
          fontSize: 12,
          decoration: 'underline',
          margin: [0, 0, 0, 10],
          bold: true
        },
        titleNoBold: {
          fontSize: 14,
          margin: [0, 0, 0, 10]
        },
        fieldName: {
          fontSize: 12,
          lineHeight: 1
        },
        text_right: {
          fontSize: 10
        },
        checklistName: {
          fontSize: 12,
          bold: true
        },
        checklistItem: {
          fontSize: 11,
          leadingIndent: 15,
          lineHeight: 1.5
        }
      },
      pageSize: 'A4',
      pageOrientation: 'portrait',
      pageMargins: [40, 80, 40, 40]
    }

    const checksdata: any = []
    report.checkLists!.forEach((checklist: CheckListQueryResult, index: number) => {
      checksdata.push(this.mapChackLists(checklist, index, report.title!.includes('v2')))
    });
    (docDef.content as any).push(checksdata)

    const principalSignature: SignatureQueryResult | undefined = report.signatures!.find((sign: SignatureQueryResult) => sign.principal);

    (docDef.content as any).push([
      {
        text: `Name of ${ResponsibleType[principalSignature!.responsibleType!]}: ${principalSignature!.responsibleName ?? '_________________'}   Designation: ${principalSignature!.designation ?? '____________________________'}`,
        style: 'filedName',
        margin: [0, 10, 0, 0]
      },
      [
        {
          text: `Date: ${moment(principalSignature!.date).format('DD/MM/YYYY hh:mm')}`,
          style: 'filedName'
        }, {
          text: 'Signature: ',
          style: 'filedName'
        },
        principalSignature!.drawnSign
          ? {
              image: `${principalSignature!.drawnSign ?? ''}`,
              width: 100,
              alignment: 'center'
            }
          : {}]
    ])

    const otherSigns = report.signatures!.filter(s => !s.principal)
    otherSigns!.forEach((sign) => {
      const imagen: any = {
        image: sign.drawnSign,
        width: 100,
        alignment: 'center'
      };

      (docDef.content as any).push([
        {
          text: `${sign.title}`, style: 'titleNoBold'
        },
        {
          text: `Name of ${ResponsibleType[sign!.responsibleType!]}: ${sign!.responsibleName ?? '____________________________'}   Designation: ${sign!.designation ?? '___________________________'}`,
          style: 'filedName',
          margin: [0, 5, 0, 0]
        },
        {
          text: 'Signature: ',
          style: 'filedName',
          margin: [0, 5, 0, 0]
        },
        sign.drawnSign ? imagen : {},
        {
          text: `Date: ${moment(sign!.date).format('DD/MM/YYYY hh:mm')}`,
          style: 'filedName',
          margin: [0, 5, 0, 0]
        },
        {
          text: `Remarks (if any): ${sign.remarks ?? ''}______________________________________________________________________________________________________________________________________________________________________________________________`,
          style: 'filedName',
          margin: [0, 5, 0, 0]
        },
        {
          text: `${sign.annotation}`, style: 'fieldName'
        }
      ])
    })
    pdfMake.createPdf(docDef).download(`report_S${report.name}`)
  }

  private mapChackLists (checklist: CheckListQueryResult, index: number, isNewFormat: boolean) {
    const mappedChecks = checklist.checks!.map((check, childIndex) => {
      return {
        pageBreak: index === 2 && childIndex === 7 && isNewFormat ? 'before' : index === 3 && childIndex === 4 && !isNewFormat ? 'before' : '', // esta mierda no me gusta muy cableado
        columns: [{ text: `    ${index + 1}.${childIndex + 1} ${check.text}`, style: 'checklistItem', width: '65%' },
          {
            stack: [
              {
                canvas: [{ type: 'rect', x: 15, y: 0, w: 30, h: 15, r: 3, color: 'white', lineColor: 'black' }],
                margin: [0, 0, 0, 10],
                width: '10%'
              },
              {
                columns: [
                  {
                    width: 30,
                    noWrap: true, // clip at with by wrapping to next line and...
                    maxHeight: 15, // don't show the wrapped text
                    svg: `${check.checked === CheckValue.Acceptable
                    ? '<svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" width="16px" height="16px" viewBox="0 0 229.153 229.153" style="enable-background:new 0 0 229.153 229.153;" xml:space="preserve"><g><path d="M92.356,223.549c7.41,7.5,23.914,5.014,25.691-6.779c11.056-73.217,66.378-134.985,108.243-193.189 C237.898,7.452,211.207-7.87,199.75,8.067C161.493,61.249,113.274,117.21,94.41,181.744 c-21.557-22.031-43.201-43.853-67.379-63.212c-15.312-12.265-37.215,9.343-21.738,21.737    C36.794,165.501,64.017,194.924,92.356,223.549z"/></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>'
                    : check.checked === CheckValue.NotAcceptableFalse
                      ? '<svg fill="#000000" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 26 26" width="26px" height="26px"><path d="M 21.734375 19.640625 L 19.636719 21.734375 C 19.253906 22.121094 18.628906 22.121094 18.242188 21.734375 L 13 16.496094 L 7.761719 21.734375 C 7.375 22.121094 6.746094 22.121094 6.363281 21.734375 L 4.265625 19.640625 C 3.878906 19.253906 3.878906 18.628906 4.265625 18.242188 L 9.503906 13 L 4.265625 7.761719 C 3.882813 7.371094 3.882813 6.742188 4.265625 6.363281 L 6.363281 4.265625 C 6.746094 3.878906 7.375 3.878906 7.761719 4.265625 L 13 9.507813 L 18.242188 4.265625 C 18.628906 3.878906 19.257813 3.878906 19.636719 4.265625 L 21.734375 6.359375 C 22.121094 6.746094 22.121094 7.375 21.738281 7.761719 L 16.496094 13 L 21.734375 18.242188 C 22.121094 18.628906 22.121094 19.253906 21.734375 19.640625 Z"/></svg>'
                      : '<svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 409.6 409.6" style="enable-background:new 0 0 409.6 409.6;" xml:space="preserve"><g><g><path d="M392.533,187.733H17.067C7.641,187.733,0,195.374,0,204.8s7.641,17.067,17.067,17.067h375.467 c9.426,0,17.067-7.641,17.067-17.067S401.959,187.733,392.533,187.733z"/></g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>'}`,
                    color: 'black'
                  }
                ],
                relativePosition: {
                  x: 22.5,
                  y: -25
                }
              }
            ]
          },
          { text: check.remarks ? check.remarks : '_________________', width: '25%' }]
      }
    })
    const checksContent = [{ text: `${index + 1}    ${checklist.text} ${checklist.annotation}        ${index === 0 ? 'Remarks' : ''}`, style: 'checklistName', margin: [0, 10, 0, 10] }, mappedChecks]
    return checksContent
  }

  public async printCompoundedPhotoRecord (reportId: number) {
    (window as any).pdfMake.vfs = pdfFonts.pdfMake.vfs

    const report = await this.getReport(reportId)

    const photos = await this.splitPhotosBy2(report.id!)
    const inspectionreport = {
      header: [{
        text: report.title,
        margin: [0, 15, 30, 0],
        style: 'name'
      },
      {
        text: report.formName,
        margin: [0, 0, 30, 0],
        style: 'subtitle'
      }],
      content: [
        {
          text: 'PARTICULARS OF INSTALLATION',
          style: 'title'
        },
        {
          text: `Name of Installation: ${report.name!.toUpperCase()}`,
          style: 'fieldName'
        },
        {
          text: `Address of Installation: ${report.address ? report.address.toUpperCase() : ''}`,
          style: 'fieldName'
        },
        {
          text: `Electrical Installation License No: ${report.licenseNumber} Date & Time of Inspection: ${moment(report.date).format('DD/MM/YYYY')}`,
          style: 'fieldName'
        }
      ]
    }

    const checksdata: any = []
    report.checkLists!.forEach((checklist: CheckListQueryResult, index: number) => {
      checksdata.push(this.mapChackLists(checklist, index, report.title!.includes('v2')))
    });
    (inspectionreport.content as any).push(checksdata)

    const principalSignature: SignatureQueryResult | undefined = report.signatures!.find((sign: SignatureQueryResult) => sign.principal);

    (inspectionreport.content as any).push([
      {
        text: `Name of ${ResponsibleType[principalSignature!.responsibleType!]}: ${principalSignature!.responsibleName ?? '___________________'}   Designation: ${principalSignature!.designation ?? '____________________________'}`,
        style: 'filedName',
        margin: [0, 10, 0, 0]
      },
      [
        {
          text: `Date: ${moment(principalSignature!.date).format('DD/MM/YYYY hh:mm')}`,
          style: 'filedName'
        }, {
          text: 'Signature: ',
          style: 'filedName'
        },
        principalSignature!.drawnSign
          ? {
              image: `${principalSignature!.drawnSign ?? ''}`,
              width: 100,
              alignment: 'center'
            }
          : {}]
    ])

    const otherSigns = report.signatures!.filter(s => !s.principal)
    otherSigns!.forEach((sign) => {
      const imagen: any = {
        image: sign.drawnSign,
        width: 100,
        alignment: 'center'
      };

      (inspectionreport.content as any).push([
        {
          text: `${sign.title}`, style: 'titleNoBold'
        },
        {
          text: `${sign.annotation}`, style: 'fieldName'
        },
        {
          text: `Name of ${ResponsibleType[sign!.responsibleType!]}: ${sign!.responsibleName ?? '______________________'}   Designation: ${sign!.designation ?? '__________________'}`,
          style: 'filedName',
          margin: [0, 5, 0, 0]
        },
        {
          text: 'Signature: ',
          style: 'filedName',
          margin: [0, 5, 0, 0]
        },
        sign.drawnSign ? imagen : {},
        {
          text: `Date: ${moment(sign!.date).format('DD/MM/YYYY hh:mm')}`,
          style: 'filedName',
          margin: [0, 5, 0, 0]
        },
        {
          text: `Remarks (if any): ${sign.remarks ?? ''}______________________________________________________________________________________________________________________________________________________________________________________________`,
          style: 'filedName',
          margin: [0, 5, 0, 0]
        }
      ])
    })

    const docDef: TDocumentDefinitions = {
      header: [{
        text: report.title!,
        margin: [0, 15, 30, 0],
        style: 'name'
      },
      {
        text: report.formName!,
        margin: [0, 0, 30, 0],
        style: 'subtitle'
      }],
      content: [
        {
          pageBreak: 'before',
          columns:
            [{
              text: 'Name of Installation',
              style: 'fieldName',
              width: '30%',
              alignment: 'left'
            },
            {
              text: ':',
              style: 'fieldName',
              width: '10%'
            },
            {
              text: `${report.name!.toUpperCase()}`,
              style: 'fieldName',
              width: '60%',
              alignment: 'left'

            }],
          margin: [40, 150, 20, 0],
          lineHeight: 2,
          alignment: 'center'
        },
        {
          columns: [
            {
              text: 'Address of Installation',
              style: 'fieldName',
              width: '30%',
              alignment: 'left'
            },
            {
              text: ':',
              style: 'fieldName',
              width: '10%'
            },
            {
              text: `${report.address ? report.address.toUpperCase() : ''}`,
              style: 'fieldName',
              width: '60%',
              alignment: 'left'
            }
          ],
          margin: [40, 0, 20, 0],
          lineHeight: 2,
          alignment: 'center'
        },
        {
          columns: [
            {
              text: 'Electrical Installation License No',
              style: 'fieldName',
              width: '30%',
              alignment: 'left'
            },
            {
              text: ':',
              style: 'fieldName',
              width: '10%'
            },
            {
              text: `${report.licenseNumber}`,
              style: 'fieldName',
              width: '60%',
              alignment: 'left'
            }
          ],
          margin: [40, 0, 20, 0],
          lineHeight: 2,
          alignment: 'center'
        },
        {
          columns: [
            {
              text: 'Date & Time of Inspection',
              style: 'fieldName',
              width: '30%',
              alignment: 'left'
            },
            {
              text: ':',
              style: 'fieldName',
              width: '10%'
            },
            {
              text: `${moment(report.date).format('DD-MMM-YYYY')}`,
              width: '60%',
              style: 'fieldName',
              alignment: 'left'
            }
          ],
          margin: [40, 0, 20, 0],
          lineHeight: 2,
          alignment: 'center'
        },
        {
          columns: [
            {
              text: 'License Validity',
              style: 'fieldName',
              width: '30%',
              alignment: 'left'
            },
            {
              text: ':',
              style: 'fieldName',
              width: '10%'
            },
            {
              text: `${moment(report.date).format('DD-MMM-YYYY')} to ${moment(report.date).format('DD-MMM-YYYY')}`,
              width: '60%',
              style: 'fieldName',
              alignment: 'left'
            }
          ],
          margin: [40, 0, 20, 0],
          lineHeight: 2,
          alignment: 'center'
        },
        {
          columns: [
            {
              text: 'Subject',
              style: 'fieldName',
              width: '30%',
              alignment: 'left'
            },
            {
              text: ':',
              style: 'fieldName',
              width: '10%'
            },
            {
              text: 'Photo Record of Inspection',
              width: '60%',
              style: 'fieldName',
              alignment: 'left'
            }
          ],
          margin: [40, 0, 20, 0],
          lineHeight: 2,
          alignment: 'center'
        },
        {
          layout: {
            hLineColor: '#32a6e4',
            vLineColor: '#32a6e4',
            hLineWidth () {
              return 2
            },
            vLineWidth () {
              return 2
            }
          },
          pageBreak: 'before',
          table: {
            headerRows: 0,
            dontBreakRows: true,
            body: photos
          },
          margin: [30, 0, 0, 0],
          alignment: 'center'
        }
      ],
      styles: {
        photoTag: {
          fontSize: 9,
          bold: true
        },
        name: {
          fontSize: 16,
          bold: true,
          alignment: 'right'

        },
        subtitle: {
          fontSize: 12,
          decoration: 'underline',
          alignment: 'right'
        },
        title: {
          fontSize: 12,
          decoration: 'underline',
          margin: [0, 0, 0, 10],
          bold: true
        },
        titleNoBold: {
          fontSize: 14,
          margin: [0, 0, 0, 10]
        },
        fieldName: {
          fontSize: 11,
          lineHeight: 1.5
        },
        text_right: {
          fontSize: 10
        },
        checklistName: {
          fontSize: 12,
          bold: true
        },
        checklistItem: {
          fontSize: 11,
          leadingIndent: 15,
          lineHeight: 1.5
        }
      },
      pageSize: 'A4',
      pageOrientation: 'portrait',
      pageMargins: [40, 80, 40, 40]
    };

    (docDef.content as any).splice(0, 0, inspectionreport.content)

    pdfMake.createPdf(docDef).download(`compunded_photo_record_${report.name}`)
  }

  private async splitPhotosBy2 (reportId: number): Promise<any> {
    const photos = await this.getReportPhotos(reportId)
    const result: any[] = []
    const tempArray: any[] = []
    for (let index = 0; index < photos.length; index++) {
      const photo = photos[index]
      tempArray.push({ stack: [{ image: `${await this.getImage(photo.photoUrl)}`, width: 220, height: 150, margin: [0, 20, 0, 20] }, { text: `${photo.label}`, alignment: 'left', style: 'photoTag' }] })
      if (index % 2 !== 0) {
        result.push([...tempArray])
        tempArray.length = 0
      } else if (index === (photos.length - 1)) {
        tempArray.push({ stack: [{ text: 'Empty', alignment: 'center' }] })
        result.push([...tempArray])
        tempArray.length = 0
      }
    }
    return result
  }
}
