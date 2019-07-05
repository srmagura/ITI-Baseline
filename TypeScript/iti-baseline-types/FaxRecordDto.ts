import { NotificationId } from './NotificationId'
import { FaxRecordId } from './FaxRecordId'
import { FaxStatus } from './FaxStatus'

export const FaxRecordDtoTypeName = 'FaxRecordDto'
export interface FaxRecordDto  { 
    id: FaxRecordId
    notificationId: NotificationId
    status: FaxStatus
    sentUtc?: Date | null
    toAddress: string
    //attachment: Int8Array
    retryCount: number
    nextRetryUtc?: Date | null
}
