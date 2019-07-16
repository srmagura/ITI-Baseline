import { NotificationId } from './NotificationId'
import { EmailRecordId } from './EmailRecordId'
import { EmailStatus } from './EmailStatus'

export const EmailRecordDtoTypeName = 'EmailRecordDto'
export interface EmailRecordDto  { 
    id: EmailRecordId
    notificationId: NotificationId
    status: EmailStatus
    sentUtc?: Date | null
    toAddress: string
    subject: string
    body: string
    retryCount: number
    nextRetryUtc?: Date | null
}
